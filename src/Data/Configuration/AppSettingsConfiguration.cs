using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Data.Entities.GeneralEntities;

namespace Data.Configuration
{
    public class AppSettingsConfiguration
    {
        public AppSettingsConfiguration(EntityTypeBuilder<AppSettings> entityTypeBuilder)
        {
            entityTypeBuilder.HasData(
                 new AppSettings { Key = "WesenderAppKey", Value = "", Description = "Acesso Wesender" });
        }
    }
}
