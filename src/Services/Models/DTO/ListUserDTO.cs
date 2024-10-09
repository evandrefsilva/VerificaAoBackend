using Data.AuthEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.DTO
{
    public class ListUserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

        public ListUserDTO() { }

        public ListUserDTO(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            //Username = Usersername;
            Email = user.Email;
            Role = user.Role?.Name;
            RoleId = user.RoleId;
            IsActive = user.IsActive;
        }
    }

}
