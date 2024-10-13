using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Models.DTO
{
    public class NewsDTOInput
    {
        [Required(ErrorMessage = "The news title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The news resume is required.")]
        public string Resume { get; set; }

        [Required(ErrorMessage = "The news text is required.")]
        public string Text { get; set; }

        public string CoverUrl { get; set; }
        public IFormFile? CoverFile { get; set; } = null;

        public DateTime PublishedDate { get; set; }

        [Required(ErrorMessage = "The tag id is required.")]
        public int CategoryId { get; set; }

        public int ReadTime { get; set; }
        public int VerficationId { get; set; }
        public int? VerificationClassificationId { get; internal set; }
        public int VerificationStatusId { get; internal set; }
        public string Obs {  get; set; }
    }

    public class VerificationDTOInput
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
