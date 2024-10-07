using Data.Entities.GeneralEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Data.AuthEntities
{
    public class User : BaseEntity
    {
        public new Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool IsEmailVerified { get; set; }
        public string VerifyCode { get; set; }
        public DateTime VerifyCodeDate { get; set; }
        public Role Role { get; set; }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

    }
}
