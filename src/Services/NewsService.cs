using ClosedXML.Excel;
using Data.Context;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Services.Clients;
using Services.Enums;
using Services.Helpers;
using Services.Models;
using Services.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.NewsVerfication;

namespace Services
{
    public interface INewsService
    {
        Task<AppResult> Create(NewsDTOInput news, CancellationToken cancellationToken = default);
        Task<AppResult> Edit(int id, NewsDTOInput news, CancellationToken cancellationToken = default);
        Task<AppResult> Approve(int id, CancellationToken cancellationToken = default);
        Task<AppResult> Reject(int id, CancellationToken cancellationToken = default);
        Task<AppResult> ForceReview(int id, CancellationToken cancellationToken = default);
        AppResult GetAll(int page = 1, int take = 30, string filter = null);
        AppResult GetAllPublished(int page = 1, int take = 30, string filter = null);
        Task<AppResult> VerifyNews(int newsId, VerificationDTOInput verificationDTO, CancellationToken cancellationToken = default);
        Task<AppResult> ChangeVerificationStatus(int newsId, int verificationStatusId, CancellationToken cancellationToken = default);

        // New methods added
        AppResult GetNewsByTag(string tag, int page = 1, int take = 30);
        Task<AppResult> GetNewsDetails(int id, CancellationToken cancellationToken = default);
        AppResult GetTags();
        AppResult GetRecentlyVerifiedPublished(int page = 1, int take = 30);
        AppResult GetPopularNews(int page = 1, int take = 30);
    }

    public class NewsService : INewsService
    {
        private readonly DataContext _db;

        public NewsService(DataContext db)
        {
            _db = db;
        }

        public async Task<AppResult> Create(NewsDTOInput dto, CancellationToken cancellationToken)
        {
            var result = new AppResult();
            try
            {
                var news = new News
                {
                    Title = dto.Title,
                    Resume = dto.Resume,
                    Text = dto.Text,
                    PublicationDate = DateTime.UtcNow,
                    IsPublished = false,
                    Like = 0,
                    UnLike = 0,
                    ReadTime = dto.ReadTime,
                    TagId = dto.TagId,
                    CoverUrl = dto.CoverUrl,
                    PublishedById = dto.UserId
                };

                // Cria a verificação com status "Em Revisão"
                var verification = new Verification
                {
                    News = news,
                    VerificationStatusId = 3, // "Em Revisão"
                    RequestedById = dto.UserId,
                    MainLink = dto.MainLink,
                    SecundaryLink = dto.SecundaryLink,
                    Obs = dto.Obs,
                    PublishedChannel = dto.PublishedChannel,
                    PublishedDate = dto.PublishedDate
                };

                // Associa a verificação à notícia
                news.Verification = verification;

                // Salva a notícia e a verificação
                await _db.News.AddAsync(news, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good(news.Id);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> Edit(int id, NewsDTOInput dto, CancellationToken cancellationToken)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (news == null)
                    return result.Bad("News not found");

                news.Title = dto.Title;
                news.Resume = dto.Resume;
                news.Text = dto.Text;
                news.PublicationDate = DateTime.UtcNow;
                news.CoverUrl = dto.CoverUrl;

                _db.News.Update(news);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good(news.Id);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> Approve(int id, CancellationToken cancellationToken)
        {
            return await ChangeStatus(id, true, cancellationToken);
        }

        public async Task<AppResult> Reject(int id, CancellationToken cancellationToken)
        {
            return await ChangeStatus(id, false, cancellationToken);
        }

        public async Task<AppResult> ForceReview(int id, CancellationToken cancellationToken)
        {
            return await ChangeVerificationStatus(id, 3, cancellationToken); // "Em Revisão"
        }

        private async Task<AppResult> ChangeStatus(int id, bool isPublished, CancellationToken cancellationToken)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (news == null)
                    return result.Bad("News not found");

                news.IsPublished = isPublished;
                news.PublicationDate = DateTime.UtcNow;

                _db.News.Update(news);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good();
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public AppResult GetAll(int page = 1, int take = 30, string filter = null)
        {
            var result = new AppResult();
            try
            {
                var query = _db.News
                    .Include(n => n.Verification)
                    .Include(n => n.Tag)
                    .Where(x => !x.IsDeleted)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query = query.Where(x => x.Title.Contains(filter) || x.Resume.Contains(filter) || x.Text.Contains(filter));
                }

                var newsList = query
                    .OrderByDescending(x => x.PublicationDate)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                return result.Good(newsList);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public AppResult GetAllPublished(int page = 1, int take = 30, string filter = null)
        {
            var result = new AppResult();
            try
            {
                var query = _db.News
                    .Include(n => n.Verification)
                    .Include(n => n.Tag)
                    .Where(x => x.IsPublished && !x.IsDeleted)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query = query.Where(x => x.Title.Contains(filter) || x.Resume.Contains(filter) || x.Text.Contains(filter));
                }

                var publishedNews = query
                    .OrderByDescending(x => x.PublicationDate)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                return result.Good(publishedNews);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> VerifyNews(int newsId, VerificationDTOInput verificationDTO, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News.FirstOrDefaultAsync(x => x.Id == newsId, cancellationToken);
                if (news == null)
                    return result.Bad("News not found");

                var verification = new Verification
                {
                    News = news,
                    VerificationStatusId = 3, // "Em Revisão"
                    RequestedById = verificationDTO.RequestedByUserId,
                    MainLink = verificationDTO.MainLink,
                    SecundaryLink = verificationDTO.SecundaryLink,
                    Obs = verificationDTO.Obs,
                    PublishedChannel = verificationDTO.PublishedChannel,
                    PublishedDate = verificationDTO.PublishedDate
                };

                await _db.Verifications.AddAsync(verification, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good(verification.Id);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> ChangeVerificationStatus(int newsId, int verificationStatusId, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News
                    .Include(n => n.Verification)
                    .FirstOrDefaultAsync(x => x.Id == newsId, cancellationToken);

                if (news == null || news.Verification == null)
                    return result.Bad("News or verification not found");

                news.Verification.VerificationStatusId = verificationStatusId;
                news.Verification.VerificationDate = DateTime.UtcNow;

                _db.Verifications.Update(news.Verification);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good(news.Verification.Id);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        // New methods implementation
        public AppResult GetNewsByTag(string tag, int page = 1, int take = 30)
        {
            var result = new AppResult();
            try
            {
                var newsList = _db.News
                    .Include(n => n.Tag)
                    .Where(n => n.Tag.Name.Equals(tag, StringComparison.OrdinalIgnoreCase) && !n.IsDeleted)
                    .OrderByDescending(n => n.PublicationDate)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                return result.Good(newsList);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> GetNewsDetails(int id, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News
                    .Include(n => n.Verification)
                    .Include(n => n.Tag)
                    .FirstOrDefaultAsync(n => n.Id == id && !n.IsDeleted, cancellationToken);

                if (news == null)
                    return result.Bad("News not found");

                return result.Good(news);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public AppResult GetTags()
        {
            var result = new AppResult();
            try
            {
                var tags = _db.Tags.ToList();
                return result.Good(tags);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public AppResult GetRecentlyVerifiedPublished(int page = 1, int take = 30)
        {
            var result = new AppResult();
            try
            {
                var newsList = _db.News
                    .Include(n => n.Verification)
                    .Where(n => n.IsPublished && n.Verification.VerificationStatusId == 1 && !n.IsDeleted) // Assuming 1 is "Verified"
                    .OrderByDescending(n => n.Verification.VerificationDate)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                return result.Good(newsList);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public AppResult GetPopularNews(int page = 1, int take = 30)
        {
            var result = new AppResult();
            try
            {
                var newsList = _db.News
                    .Where(n => n.IsPublished && !n.IsDeleted)
                    .OrderByDescending(n => n.Like - n.UnLike) // Assuming popularity is based on likes minus unlikes
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToList();

                return result.Good(newsList);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }
    }
}
