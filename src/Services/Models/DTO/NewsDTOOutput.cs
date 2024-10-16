using Data.NewsVerfication;
using DocumentFormat.OpenXml.Office2016.Excel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Services.Models.DTO
{
    public class NewsDTOOutput
    {
        public NewsDTOOutput(News news, string baseUrl)
        {
            Id = news.Id;
            Title = news.Title;
            Resume = news.Resume;
            PublicationDate = news.PublicationDate;
            IsPublished = news.IsPublished;
            Like = news.Like;
            UnLike = news.UnLike;
            ReadTime = news.ReadTime;
            PublishedBy = $"{news.PublishedBy?.FirstName} {news.PublishedBy?.LastName}";
            CategoryName = news.Category?.Name;
            CategorySlug = news.Category?.Slug;
            CategoryId = news.CategoryId;
            VerificationStatus = news.Verification?.VerificationStatus?.Name;
            VerificationStatusId = news.VerificationId;
            VerificationClassification = news.Verification?.VerificationClassification?.Name;
            VerificationClassificationId = news.Verification.VerificationClassificationId;
            CoverUrl = !string.IsNullOrEmpty(news.CoverUrl) ? $"{baseUrl}{news.CoverUrl}" : null;
        }
        public NewsDTOOutput(News news) : this(news, "")
        {

        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Resume { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool IsPublished { get; set; }
        public int Like { get; set; }
        public int UnLike { get; set; }
        public int ReadTime { get; set; }
        public string PublishedBy { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }
        public string VerificationStatus { get; set; }
        public int VerificationStatusId { get; set; }
        public string VerificationClassification { get; set; }
        public int? VerificationClassificationId { get; set; }
        public string CoverUrl { get; set; }
    }

    public class NewsDetailsDTOOutput
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Resume { get; set; }

        public string Text { get; set; }

        public DateTime PublicationDate { get; set; }

        public bool IsPublished { get; set; } // Status de publicação

        public int Like { get; set; } // Número de likes

        public int UnLike { get; set; } // Número de dislikes

        public int ReadTime { get; set; } // Tempo de leitura estimado

        public string PublishedBy { get; set; } // Nome do usuário que publicou a notícia

        public string TagName { get; set; } // Nome da tag associada

        public VerificationDTOOutput Verification { get; set; } // Detalhes da verificação associada à notícia
    }

}
