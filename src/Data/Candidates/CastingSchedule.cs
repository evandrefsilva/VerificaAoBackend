using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Candidates
{
    public class CastingSchedule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        [Required]
        public int ProvinceId { get; set; }

        [Required]
        public int Capacity { get; set; }
        public Province Province { get; set; }
        public List<Candidate> Candidates {get;set;}
    }
}
