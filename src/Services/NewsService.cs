using Data.Context;
using Microsoft.EntityFrameworkCore;
using Services.Models.DTO;
using Services.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Data.NewsVerfication;
using Services.Models;

namespace Services
{
    public interface INewsService
    {
        Task<AppResult> CreateNews(CreateOrUpdateNewsDTO newsDTO, Guid publishedById, CancellationToken cancellationToken = default);
        Task<AppResult> GetAllNews(PaginationFilterParameters pagination, bool onlyPublished = false, CancellationToken cancellationToken = default);
        Task<AppResult> GetNewsByCategorySlug(string categorySlug, PaginationFilterParameters pagination, bool onlyPublished = false, CancellationToken cancellationToken = default);
        Task<AppResult> GetNewsDetailsById(int id, CancellationToken cancellationToken = default);
        Task<AppResult> GetNewsDetailsBySlug(string slug, CancellationToken cancellationToken = default);
        Task<AppResult> GetPopularNews(PaginationParameters pagination, CancellationToken cancellationToken = default);
        Task<AppResult> EditNews(int id, CreateOrUpdateNewsDTO newsDTO, CancellationToken cancellationToken = default);
        Task<AppResult> DeleteNews(int id, CancellationToken cancellationToken = default);
        Task<AppResult> LikeNews(LikeDTO dto, CancellationToken cancellationToken = default);
        Task<AppResult> UnlikeNews(UnlikeDTO dto, CancellationToken cancellationToken = default);
        Task<AppResult> TogglePublishStatus(int newsId, CancellationToken cancellationToken = default);
    }

    public class NewsService : INewsService
    {
        private readonly DataContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl;

        public NewsService(DataContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _baseUrl = _httpContextAccessor.GetBaseUrl();
        }

        public async Task<AppResult> CreateNews(CreateOrUpdateNewsDTO dto, Guid publishedById,
            CancellationToken cancellationToken)
        {
            var result = new AppResult();

            var verification = await _db.Verifications
                .FirstOrDefaultAsync(v => v.Id == dto.VerficationId, cancellationToken);

            if (verification == null)
                return result.Bad("Verificação inválida.");
            if (verification.News != null)
                return result.Bad("Já existe uma notícia associada a esta verificação.");

            try
            {
                var news = new News
                {
                    Title = dto.Title,
                    Slug = SlugHelper.GenerateSlug(dto.Title),
                    Resume = dto.Resume,
                    Text = dto.Text,
                    CoverUrl = dto.CoverUrl,
                    PublicationDate = DateTime.UtcNow,
                    IsPublished = false,
                    Like = 0,
                    UnLike = 0,
                    ReadTime = dto.ReadTime,
                    CategoryId = dto.CategoryId,
                    PublishedById = publishedById,
                    VerificationId = dto.VerficationId
                };

                _db.News.Add(news);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good(news.Id);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> GetAllNews(PaginationFilterParameters page,
            bool onlyPublished = false, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var query = _db.News
                    .Include(n => n.Verification.VerificationStatus)
                    .Include(n => n.Verification.VerificationClassification)
                    .Include(n => n.Category)
                    .Include(n => n.PublishedBy)
                    .Where(x => !x.IsDeleted)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(page.filter))
                {
                    query = query.Where(x => x.Title.Contains(page.filter) || x.Resume.Contains(page.filter));
                }
                if (onlyPublished)
                {
                    query = query.Where(n => n.IsPublished);
                }

                var newsList = await query
                    .OrderByDescending(x => x.PublicationDate)
                    .Select(x => new NewsDTOOutput(x, _baseUrl))
                    .ToPagedList(page.page, page.take, cancellationToken);

                return result.Good(newsList);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> GetPopularNews(PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var newsList = await _db.News
                    .Include(n => n.Verification.VerificationStatus)
                    .Include(n => n.Verification.VerificationClassification)
                    .Include(n => n.Category)
                    .OrderByDescending(n => n.Like - n.UnLike)
                    .Select(n => new NewsDTOOutput(n, _baseUrl))
                    .ToPagedList(pagination.page, pagination.take, cancellationToken);

                return result.Good(newsList);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> EditNews(int id, CreateOrUpdateNewsDTO dto, CancellationToken cancellationToken)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News.FirstOrDefaultAsync(x => x.Id == id

 && !x.IsDeleted, cancellationToken);

                if (news == null)
                    return result.Bad("Notícia não encontrada.");

                news.Title = dto.Title;
                news.Resume = dto.Resume;
                news.Text = dto.Text;
                news.CoverUrl = dto.CoverUrl;
                news.ReadTime = dto.ReadTime;
                news.CategoryId = dto.CategoryId;

                _db.News.Update(news);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good("Notícia atualizada com sucesso.");
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> DeleteNews(int id, CancellationToken cancellationToken)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);

                if (news == null)
                    return result.Bad("Notícia não encontrada.");

                news.IsDeleted = true;
                _db.News.Update(news);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good("Notícia excluída com sucesso.");
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> LikeNews(LikeDTO dto, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News.FirstOrDefaultAsync(x => x.Id == dto.NewsId && !x.IsDeleted, cancellationToken);
                if (news == null)
                    return result.Bad("Notícia não encontrada.");

                news.Like++;
                _db.News.Update(news);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good("Gostou da notícia.");
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> UnlikeNews(UnlikeDTO dto, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News.FirstOrDefaultAsync(x => x.Id == dto.NewsId && !x.IsDeleted, cancellationToken);
                if (news == null)
                    return result.Bad("Notícia não encontrada.");

                news.UnLike++;
                _db.News.Update(news);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good("Desgostou da notícia.");
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> TogglePublishStatus(int newsId, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News.FirstOrDefaultAsync(x => x.Id == newsId && !x.IsDeleted, cancellationToken);
                if (news == null)
                    return result.Bad("Notícia não encontrada.");

                news.IsPublished = !news.IsPublished;
                _db.News.Update(news);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good(news.IsPublished ? "Notícia publicada." : "Notícia despublicada.");
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }
        public async Task<AppResult> GetNewsDetailsBySlug(string slug, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News
                    .Include(n => n.Verification.VerifiedBy)
                    .Include(n => n.Verification.VerificationClassification)
                    .Include(n => n.Category)
                    .Include(n => n.PublishedBy)
                    .FirstOrDefaultAsync(n => n.Slug == slug && !n.IsDeleted, cancellationToken);

                if (news == null)
                    return result.Bad("Notícia não encontrada.");
                var newsDTO = new NewsDetailDTO(news,_baseUrl);

                return result.Good(newsDTO);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }
        public async Task<AppResult> GetNewsDetailsById(int id, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News
                    .Include(n => n.Verification.VerifiedBy)
                    .Include(n => n.Verification.VerificationClassification)
                    .Include(n => n.Category)
                    .Include(n => n.PublishedBy)
                    .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted, cancellationToken);

                if (news == null)
                    return result.Bad("Notícia não encontrada.");
                var newsDTO = new NewsDetailDTO(news, _baseUrl);

                return result.Good(newsDTO);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> GetNewsByCategorySlug(string categorySlug, PaginationFilterParameters page,
            bool onlyPublished = false, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var query = _db.News
                    .Include(n => n.Category)
                    .Include(n => n.Verification.VerificationStatus)
                    .Include(n => n.Verification.VerificationClassification)
                    .Include(n => n.Category)
                    .Include(n => n.PublishedBy)
                    .Where(x => !x.IsDeleted && x.Category.Slug == categorySlug)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(page.filter))
                {
                    query = query.Where(x => x.Title.Contains(page.filter) || x.Resume.Contains(page.filter));
                }
                if (onlyPublished)
                {
                    query = query.Where(n => n.IsPublished);
                }

                var newsList = await query
                    .OrderByDescending(x => x.PublicationDate)
                    .Skip((page.page - 1) * page.take)
                    .Take(page.take)
                    .Select(x => new NewsDTOOutput(x, _baseUrl))
                    .ToPagedList(page.page, page.take, cancellationToken);

                return result.Good(newsList);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }

        }
    }
}