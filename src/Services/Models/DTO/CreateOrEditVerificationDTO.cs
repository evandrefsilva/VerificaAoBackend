using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Services.Models.DTO
{
    public class CreateOrEditVerificationDTO
    {
        public string MainLink { get; set; }
        public string SecundaryLink { get; set; }
        public string Obs { get; set; }
        public DateTime PublishedDate { get; set; }
        public string PublishedTitle { get; set; }
        public string PublishedChannel { get; set; }
        public List<IFormFile> Attachment { get; set; }
        public Guid? VerifiedById { get; set; }
    }

}
