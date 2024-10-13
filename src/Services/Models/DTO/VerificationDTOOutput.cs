using Data.Candidates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models.DTO
{
    public class VerificationDTOOutput
    {
        public int Id { get; set; }
        public int VerificationStatusId { get; set; }
        public int VerificationClassificationId { get; set; }
        public string VerificationStatus { get; set; }
        public string VerificationClassification { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedDate { get; set; }
        public string MainLink { get; set; }
        public string SecundaryLink { get; set; }
        public string Obs { get; set; }
        public string PublishedTile { get; set; }
        public string PublishedChannel { get; set; }
        public DateTime PublishedDate { get; set; }
        public string VerifiedByName { get; set; }
    }
}