using Data.NewsVerfication;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configuration
{
    public class VerificationClassificationConfiguration
    {
        public VerificationClassificationConfiguration(EntityTypeBuilder<VerificationClassification> builder)
        {
            builder.HasData(
                new VerificationClassification { Id = 1, Name = "Fake News" },
                new VerificationClassification { Id = 2, Name = "Verídico" },
                new VerificationClassification { Id = 3, Name = "Enganoso" }
            );
        }
    }
}
