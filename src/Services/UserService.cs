using System;
using System.Linq;
using Data;
using Services.Models;
using Services.Models.DTO;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using Data.Context;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Data.AuthEntities;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;

namespace Services
{
    public interface IUserService
    {
        AppResult Login(LoginDTO dto);
        Task<AppResult> Register(RegisterUserDTO dto);
        Task<AppResult> RecoverPassword(RecoveryPasswordDTO dto);
        Task<AppResult> RequestVerifyEmail(RequestVerifyEmailDTO dto);
        Task<AppResult> VerifyEmail(VerifyEmailDTO dto);
        Task<AppResult> UpdatePassword(UpdatePasswordDTO dto);

        // Métodos adicionais para gestão de utilizadores e perfis
        Task<AppResult> ListUsers(int page = 1, int take = 30);
        Task<AppResult> CreateUser(CreateUserDTO dto);
        Task<AppResult> EditUser(EditUserDTO dto);
        Task<AppResult> DeleteUser(int userId);

        Task<AppResult> ListRoles();
        Task<AppResult> ListPermissions();
        Task<AppResult> CreateRole(CreateRoleDTO dto);
        Task<AppResult> EditRole(EditRoleDTO dto);
        Task<AppResult> DeleteRole(int roleId);
    }

    public class UserService : IUserService
    {
        private readonly DataContext _db;
        private readonly string _jwtSecret;

        public UserService(DataContext db)
        {
            _db = db;
            _jwtSecret = "secret";
        }

        public AppResult Login(LoginDTO dto)
        {
            var res = new AppResult();
            var user = _db.Users
                .FirstOrDefault(x => x.Username == dto.Email
                   && x.Password == dto.Password.ToSha512Hash());

            if (user == null)
                return res.Bad("Login inválido");

            // Geração do token JWT
            var token = GenerateJwtToken(user);
            return res.Good("Login feito com sucesso.", new ReturnLoginDTO
            {
                Name = user.FirstName,
                UserId = user.Id,
                Email = user.Email,
                Token = token
            });
        }

        public async Task<AppResult> Register(RegisterUserDTO dto)
        {
            var res = new AppResult();
            var existingUser = await _db.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existingUser != null)
                return res.Bad("Usuário já existe com esse e-mail ou nome de usuário.");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Email,
                Email = dto.Email,
                Password = dto.Password.ToSha512Hash(),
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            return res.Good("Usuário registrado com sucesso.");
        }

        public async Task<AppResult> RecoverPassword(RecoveryPasswordDTO dto)
        {
            var res = new AppResult();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null)
                return res.Bad("E-mail não encontrado.");

            // Aqui você pode implementar a lógica de recuperação de senha (e.g., enviar e-mail com um link de redefinição)
            return res.Good("Instruções de recuperação de senha enviadas para o seu e-mail.");
        }

        public async Task<AppResult> RequestVerifyEmail(RequestVerifyEmailDTO dto)
        {
            var res = new AppResult();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return res.Bad("Usuário não encontrado.");

            // Gerar código de verificação (pode ser um código simples ou algo mais complexo)
            var verifyCode = new Random().Next(100000, 999999).ToString();

            user.VerifyCode = verifyCode;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            // Lógica para enviar e-mail
            var subject = "Seu código de verificação";
            var body = $"Seu código de verificação é: {verifyCode}";

            return res.Good("Código de verificação enviado.");
        }
        public async Task<AppResult> RequestUpdatePassword(RequestVerifyEmailDTO dto)
        {
            var res = new AppResult();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return res.Bad("Usuário não encontrado.");

            // Gerar código de verificação (pode ser um código simples ou algo mais complexo)
            var verifyCode = new Random().Next(100000, 999999).ToString();

            user.VerifyCode = verifyCode;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            // Lógica para enviar e-mail
            var subject = "Use o código abaixo";
            var body = $"Use o código abaixo para verficar o senhor: {verifyCode}";

            return res.Good("Código de verificação enviado.");
        }

        public async Task<AppResult> VerifyEmail(VerifyEmailDTO dto)
        {
            var res = new AppResult();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return res.Bad("Usuário não encontrado.");

            if (user.VerifyCode != dto.VerifyCode)
                return res.Bad("Código de verificação inválido.");

            user.IsEmailVerified = true;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return res.Good("E-mail verificado com sucesso.");
        }


        public async Task<AppResult> UpdatePassword(UpdatePasswordDTO dto)
        {
            var res = new AppResult();
            var user = await _db.Users.FirstOrDefaultAsync(x => x.VerifyCode == dto.VerifyCode);

            if (user == null)
                return res.Bad("Usuário não encontrado.");

            //if ()
            //    return res.Bad("Senha antiga incorreta.");

            if (dto.VerifyCode != user.VerifyCode) // Exemplo de verificação
                return res.Bad("Código de verificação inválido.");

            user.Password = dto.Password.ToSha512Hash();
            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return res.Good("Senha atualizada com sucesso.");
        }
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // Métodos para listar utilizadores
        public async Task<AppResult> ListUsers(int page = 1, int take = 30)
        {
            var users = await _db.Users
                .Include(u => u.Role)
                .Select(u => new ListUserDTO(u))
                .Skip((page - 1) * take)
                 .Take(take)
                .ToListAsync();
            return new AppResult().Good("Listagem de utilizadores.", users);
        }

        // Criar novo utilizador
        public async Task<AppResult> CreateUser(CreateUserDTO dto)
        {
            var res = new AppResult();
            var existingUser = await _db.Users
                .FirstOrDefaultAsync(x => x.Email == dto.Email || x.Username == dto.Username);

            if (existingUser != null)
                return res.Bad("Este e-mail ou nome de utilizador já está registado.");

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password.ToSha512Hash(),
                RoleId = dto.RoleId,
                IsEmailVerified = true,
                VerifyCode = null,
                VerifyCodeDate = DateTime.UtcNow.AddMinutes(10)
            };

            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();

            // Enviar e-mail de verificação aqui (opcional)

            return res.Good("Utilizador criado com sucesso.", user.Id);
        }

        // Editar utilizador existente
        public async Task<AppResult> EditUser(EditUserDTO dto)
        {
            var res = new AppResult();
            var user = await _db.Users.FindAsync(dto.UserId);

            if (user == null)
                return res.Bad("Utilizador não encontrado.");

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Username = dto.Username;
            user.RoleId = dto.RoleId;

            _db.Users.Update(user);
            await _db.SaveChangesAsync();

            return res.Good("Utilizador atualizado com sucesso.");
        }

        // Apagar utilizador
        public async Task<AppResult> DeleteUser(int userId)
        {
            var res = new AppResult();
            var user = await _db.Users.FindAsync(userId);

            if (user == null)
                return res.Bad("Utilizador não encontrado.");

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();

            return res.Good("Utilizador apagado com sucesso.");
        }

        // Listar perfis (roles)
        public async Task<AppResult> ListRoles()
        {
            var roles = await _db.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();
            return new AppResult().Good("Listagem de perfis.", roles);
        }
        // Listar perfis (roles)
        public async Task<AppResult> ListPermissions()
        {
            var roles = await _db.Permissions
                .ToListAsync();
            return new AppResult().Good("Listagem de permissoes.", roles);
        }

        // Criar perfil com permissões
        public async Task<AppResult> CreateRole(CreateRoleDTO dto)
        {
            var res = new AppResult();

            // Verificar se o perfil já existe
            var existingRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name == dto.Name);
            if (existingRole != null)
                return res.Bad("Perfil com este nome já existe.");

            var role = new Role
            {
                Name = dto.Name,
                RolePermissions = new List<RolePermission>()
            };

            foreach (var permissionId in dto.PermissionIds)
            {
                var permission = await _db.Permissions.FindAsync(permissionId);
                if (permission != null)
                {
                    role.RolePermissions.Add(new RolePermission
                    {
                        PermissionId = permissionId
                    });
                }
            }

            await _db.Roles.AddAsync(role);
            await _db.SaveChangesAsync();

            return res.Good("Perfil criado com sucesso.", role.Id);
        }

        // Editar perfil e suas permissões
        public async Task<AppResult> EditRole(EditRoleDTO dto)
        {
            var res = new AppResult();
            var role = await _db.Roles
                .Include(r => r.RolePermissions)
                .FirstOrDefaultAsync(r => r.Id == dto.RoleId);

            if (role == null)
                return res.Bad("Perfil não encontrado.");

            role.Name = dto.Name;

            // Atualizar permissões
            // Remove permissões antigas
            _db.RolePermissions.RemoveRange(role.RolePermissions);

            // Adiciona novas permissões
            foreach (var permissionId in dto.PermissionIds)
            {
                var permission = await _db.Permissions.FindAsync(permissionId);
                if (permission != null)
                {
                    role.RolePermissions.Add(new RolePermission
                    {
                        PermissionId = permissionId
                    });
                }
            }

            _db.Roles.Update(role);
            await _db.SaveChangesAsync();

            return res.Good("Perfil atualizado com sucesso.");
        }

        // Apagar perfil
        public async Task<AppResult> DeleteRole(int roleId)
        {
            var res = new AppResult();
            var role = await _db.Roles.FindAsync(roleId);

            if (role == null)
                return res.Bad("Perfil não encontrado.");

            // Remover as associações de permissões
            var rolePermissions = _db.RolePermissions.Where(rp => rp.RoleId == roleId);
            _db.RolePermissions.RemoveRange(rolePermissions);

            _db.Roles.Remove(role);
            await _db.SaveChangesAsync();

            return res.Good("Perfil apagado com sucesso.");
        }



    }
}
