using Data.AuthEntities;
using Data.Entities.GeneralEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.NewsVerfication
{
    public class Verification : BaseEntity
    {
        public int VerificationStatusId { get; set; }
        public VerificationStatus VerificationStatus { get; set; }

        public Guid? VerifiedById { get; set; }
        public User VerifiedBy { get; set; }

        public Guid RequestedById { get; set; }
        public User RequestedBy { get; set; }

        public News News { get; set; }

        public string MainLink { get; set; }
        public string SecundaryLink { get; set; }
        public string Obs { get; set; }
        public string Attachment { get; set; }

        public string PublishedTitle { get; set; }
        public string PublishedChannel { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
        public DateTime? VerificationDate { get; set; }
    }
}
