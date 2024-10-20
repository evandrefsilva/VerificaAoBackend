using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Services.Models.DTO
{
    public class CreateOrUpdateNewsDTO
    {
        [Required(ErrorMessage = "The news title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The news resume is required.")]
        public string Resume { get; set; }

        [Required(ErrorMessage = "The news text is required.")]
        public string Text { get; set; }

        public string CoverUrl { get; set; }
        public IFormFile? CoverFile { get; set; } = null;

        [Required(ErrorMessage = "The tag id is required.")]
        public int CategoryId { get; set; }

        public int ReadTime { get; set; }
        public int VerficationId { get; set; }
        public int? VerificationClassificationId { get; set; }
        public int VerificationStatusId { get; set; }
        public string Obs {  get; set; }
    }

}
