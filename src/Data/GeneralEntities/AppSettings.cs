using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities.GeneralEntities
{
   public class AppSettings
    {
       
        [MaxLength(50)]
        [Key]
        public string Key { get; set; }
        [MaxLength(4000)]
        public string Value { get; set; }
        [MaxLength(4000)]
        public string Description { get; set; }
    }
}
