using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Models.DTO
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "O primeiro nome é obrigatório.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "O sobrenome é obrigatório.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "O nome de utilizador é obrigatório.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O perfil é obrigatório.")]
        public int RoleId { get; set; } // ID do perfil a ser associado
    }

    public class EditUserDTO
    {
        [Required(ErrorMessage = "O ID do utilizador é obrigatório.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "O primeiro nome é obrigatório.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "O sobrenome é obrigatório.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O nome de utilizador é obrigatório.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "O perfil é obrigatório.")]
        public int RoleId { get; set; } // ID do perfil a ser associado
    }

    public class CreateRoleDTO
    {
        [Required(ErrorMessage = "O nome do perfil é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ao menos uma permissão deve ser selecionada.")]
        public List<int> PermissionIds { get; set; } // IDs das permissões a serem associadas
    }

    public class EditRoleDTO
    {
        [Required(ErrorMessage = "O ID do perfil é obrigatório.")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "O nome do perfil é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ao menos uma permissão deve ser selecionada.")]
        public List<int> PermissionIds { get; set; } // IDs das permissões a serem associadas
    }
}
