using Data.Entities.GeneralEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.NewsVerfication
{
    public class VerificationStatus
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
