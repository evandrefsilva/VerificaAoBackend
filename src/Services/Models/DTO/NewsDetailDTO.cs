using Data.NewsVerfication;
using System;
using System.Text;

namespace Services.Models.DTO
{
    public class NewsDetailDTO
    {
        public NewsDetailDTO(News news, string baseUrl)
        {
            Id = news.Id;
            Title = news.Title;
            Text = news.Text;
            Resume = news.Resume;
            Slug = news.Slug;
            CoverUrl = !string.IsNullOrEmpty(news.CoverUrl) ? $"{baseUrl}{news.CoverUrl}" : null;
            PublishedDate = news.PublicationDate;
            Like = news.Like;
            UnLike = news.UnLike;
            CategoryName = news.Category.Name;
            CategorySlug = news.Category.Slug;
            PublishedByName = news.PublishedBy.GetFullName();
            VerifiedByName = news.Verification.VerifiedBy.GetFullName();
            VerifiedByProfilePicture = !string.IsNullOrEmpty(news.Verification?.VerifiedBy?.ProfilePicture) 
                ? $"{baseUrl}{news.Verification?.VerifiedBy?.ProfilePicture}" : null;
            VerifiedByAbout = news.Verification.VerifiedBy.About;
            VerificationClassification = news.Verification.VerificationClassification?.Name;
            VerificationClassificationId = news.Verification.VerificationClassificationId;

        }
        public NewsDetailDTO(News news):this(news,"")
        {
             
        }
        // Campos da entidade News
        public int Id { get; set; }
        public string Title { get; set; }
        public string Resume { get; set; }
        public string Slug { get; set; }
        public string Text { get; set; }
        public string CoverUrl { get; set; }
        public DateTime? PublishedDate { get; set; }
        public int Like { get; set; }
        public int UnLike { get; set; }
        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }

        // Informações do autor
        public string PublishedByName { get; set; }

        // Informações de verificação (Verified By)
        public string VerifiedByName { get; set; }
        public string VerifiedByProfilePicture { get; set; }
        public string VerifiedByAbout { get; set; }
        // Classificacao
        public string VerificationClassification { get; set; }
        public int? VerificationClassificationId { get; set; }

    }

}
