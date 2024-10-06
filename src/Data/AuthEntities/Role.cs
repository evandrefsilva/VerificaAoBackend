using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.AuthEntities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Slug { get; set; }
    }
}
