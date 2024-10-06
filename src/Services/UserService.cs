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

namespace Services
{
    public interface IUserService
    {
        AppResult Login(LoginDTO dto);
        Task<AppResult> Register(RegisterUserDTO dto);
        Task<AppResult> RecoverPassword(RecoveryPasswordDTO dto);
        Task<AppResult> RequestVerifyEmail(RequestVerifyEmailDTO dto);
        Task<AppResult> VerifyEmail(VerifyEmailDTO dto);
        Task<AppResult> UpdatePassword(UpdatePasswordDTO dto); // Novo método para atualizar a senha
    }

    public class UserService : IUserService
    {
        private readonly DataContext _db;
        private readonly string _jwtSecret;

        public UserService(DataContext db, string jwtSecret)
        {
            _db = db;
            _jwtSecret = jwtSecret;
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
                Password = dto.Password.ToSha512Hash()
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
    }
}
