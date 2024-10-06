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
                new VerificationStatus { Id = 1, Name = "Verdadeira"},
                new VerificationStatus { Id = 2, Name = "Falsa" },
                new VerificationStatus { Id = 3, Name = "Em Revisao" }
            );
        }
    }
}
