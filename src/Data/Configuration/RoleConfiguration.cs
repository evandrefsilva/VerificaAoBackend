using Data.AuthEntities;
using Data.NewsVerfication;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    public class RoleConfiguration
    {
        public RoleConfiguration(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role { Id = 1, Name = "Administrador"},
                new Role { Id = 2, Name = "Gestor" },
                new Role { Id = 3, Name = "Publicador" },
                new Role { Id = 4, Name = "Revisador" },
                new Role { Id = 5, Name = "User" }
            );
        }
    }
}
