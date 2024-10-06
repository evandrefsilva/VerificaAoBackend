using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Candidates
{
    public class Province
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da provincia é obrigatório.")]
        public string Name { get; set; }
        public string ProvincePrefix { get; set; }
    }
}
