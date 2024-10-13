using Data.NewsVerfication;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    public class VerificationStatusConfiguration
    {
        public VerificationStatusConfiguration(EntityTypeBuilder<VerificationStatus> builder)
        {
            builder.HasData(
                new VerificationStatus { Id = 1, Name = "Activo no Portal" },
                new VerificationStatus { Id = 2, Name = "Verificado" },
                new VerificationStatus { Id = 3, Name = "Aguardando Verificação" }
            );
        }
    }
}
