using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Data.AuthEntities;

namespace Data.Configuration
{
    public class UserConfiguration
    {
        public UserConfiguration(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User
            {

                Id = Guid.Parse(Guid.Empty.ToString().Replace("0", "1")),
                Username = "admin",
                FirstName = "Administrador de Sistema",
                LastName = "Administrador de Sistema",
                Password = "0f3d85258d593088098f65c26e89d49bf1bcf29b2b57dcfc36865ecefec7551fb8f232a028d98bd39acfb2710ef5e6e8f08e5a4ddbc213a82ad6008e64861abd",
                Email = "admin@admin.com",
                IsActive = true,
                //admin
                RoleId = 1
            });
        }
    }
}
