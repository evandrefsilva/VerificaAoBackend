using Data.AuthEntities;
using Data.Entities.GeneralEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.NewsVerfication
{
    public class News : BaseEntity
    {
        [Required(ErrorMessage = "Title of the news is required.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Resume of the news is required.")]
        public string Resume { get; set; }
        [Required(ErrorMessage = "Text of the news is required.")]
        public string Text { get; set; }
        public string CoverUrl { get; set; }
        public DateTime PublicationDate { get; set; }

        public int Like { get; set; } = 0;
        public int UnLike { get; set; } = 0;

        public bool IsPublished { get; set; }
        public Guid PublishedById { get; set; }
        public User PublishedBy { get; set; }

        public int ReadTime { get; set; }
        public int VerificationId { get; set; }
        public virtual Verification Verification { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }

}
