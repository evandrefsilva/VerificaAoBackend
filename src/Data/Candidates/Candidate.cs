using Data.Entities.GeneralEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Candidates
{
    public class Candidate : BaseEntity
    {
        [Column(Order = 1)]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Informe seu nome por favor")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Informe seu sobrenome por favor")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Informe seu cantor por favor")]
        public string Singer { get; set; }

        [Required(ErrorMessage = "Informe a primeira música por favor")]
        public string Musics1 { get; set; }
        [Required(ErrorMessage = "Informe a segunda música por favor")]
        public string Musics2 { get; set; }
        [Required(ErrorMessage = "Informe a terceira música por favor")]
        public string Musics3 { get; set; }

        [Phone(ErrorMessage = "Insira um número de telefone válido")]
        public string Contact1 { get; set; }

        [Phone(ErrorMessage = "Insira um número de telefone válido")]
        public string Contact2 { get; set; }

        [Required(ErrorMessage = "Informe a data de nascimento por favor")]
        public string DateBorn { get; set; }

        [Required(ErrorMessage = "Informe a província de nascimento por favor")]
        public int ProvinceBornId { get; set; }
        public int ProvinceId { get; set; }
        public int CandidatureStatusId { get; set; }
        public int? CastingScheduleId { get; set; }
        public Province ProvinceBorn { get; set; }
        public Province Province { get; set; }
        [ForeignKey("CastingScheduleId")]
        public CastingSchedule CastingSchedule { get; set; }
    }

}
