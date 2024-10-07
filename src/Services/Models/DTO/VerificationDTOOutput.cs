using Data.Candidates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.DTO
{
    public class VerificationDTOOutput
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public int VerificationStatusId { get; set; }
        public string VerificationStatus { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedDate { get; set; }
        public string MainLink { get; set; }
        public string SecundaryLink { get; set; }
        public string Obs { get; set; }
        public string PublishedChannel { get; set; }
    }
}