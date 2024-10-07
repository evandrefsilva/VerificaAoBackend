using Data.Context;
using Microsoft.EntityFrameworkCore;
using Services.Enums;
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
        Task<AppResult> Create(NewsDTOInput news, Guid publishedId, CancellationToken cancellationToken = default);
        Task<AppResult> CreateVerification(VerificationDTOInput news, Guid requestedById, CancellationToken cancellationToken = default);
        Task<AppResult> Edit(int id, NewsDTOInput news, CancellationToken cancellationToken = default);
        Task<AppResult> Delete(int id, CancellationToken cancellationToken = default);
        Task<AppResult> Approve(int id, CancellationToken cancellationToken = default);
        Task<AppResult> Reject(int id, CancellationToken cancellationToken = default);
        Task<AppResult> ForceReview(int id, CancellationToken cancellationToken = default);
        Task<AppResult> GetAll(int page = 1, int take = 30, string filter = null, CancellationToken cancellationToken = default);
        Task<AppResult> GetAllPublished(int page = 1, int take = 30, string filter = null, CancellationToken cancellationToken = default);
        Task<AppResult> GetPopularNews(int page = 1, int take = 30, CancellationToken cancellationToken = default);
        //Task<AppResult> VerifyNews(int newsId, VerificationDTOInput verificationDTO, CancellationToken cancellationToken = default);
        Task<AppResult> GetAllVerfications(int page = 1, int take = 30, CancellationToken cancellationToken = default);
        Task<AppResult> ChangeVerificationStatus(int newsId, int verificationStatusId, CancellationToken cancellationToken = default);

        Task<AppResult> GetNewsByTag(string tag, int page = 1, int take = 30, CancellationToken cancellationToken = default);
        Task<AppResult> GetNewsDetails(int id, CancellationToken cancellationToken = default);
        Task<AppResult> GetRecentlyVerifiedPublished(int page = 1, int take = 30, CancellationToken cancellationToken = default);

        Task<AppResult> Unlike(UnlikeDTO dto, CancellationToken cancellationToken = default);
        Task<AppResult> Like(LikeDTO dto, CancellationToken cancellationToken = default);
    }

    public class NewsService : INewsService
    {
        private readonly DataContext _db;

        public NewsService(DataContext db)
        {
            _db = db;
        }
        public async Task<AppResult> Create(NewsDTOInput dto, Guid publishedId, CancellationToken cancellationToken)
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
                    PublishedById = publishedId
                };

                // Salva a notícia no banco
                await _db.News.AddAsync(news, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good(news.Id);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }
        public async Task<AppResult> GetAllVerfications(int page = 1, int take = 30, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var verifications = await _db.Verifications
                    .Include(v => v.News) // Inclui os dados da notícia relacionada
                    .Include(v => v.VerificationStatus) // Inclui o status da verificação
                    .Skip((page - 1) * take)
                    .Take(take)
                    .Select(v => new VerificationDTOOutput
                    {
                        Id = v.Id,
                        NewsTitle = v.News.Title,
                        VerificationStatusId = v.VerificationStatusId,
                        VerificationStatus = v.VerificationStatus.Name, // Assumindo que exista uma propriedade `Name`
                        RequestedBy = v.RequestedBy.Username, // Assumindo que o Requester seja um User
                        RequestedDate = v.CreatedAt, // Propriedade que armazena a data da solicitação
                        MainLink = v.MainLink,
                        SecundaryLink = v.SecundaryLink,
                        Obs = v.Obs,
                        PublishedChannel = v.PublishedChannel
                    })
                    .ToListAsync(cancellationToken);

                return result.Good(verifications);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> CreateVerification(VerificationDTOInput dto, Guid requestedById, CancellationToken cancellationToken)
        {
            var result = new AppResult();
            try
            {
                var verification = new Verification
                {
                    PublishedTitle = dto.PublishedTitle,
                    VerificationStatusId = (int)VerificationStatusEnum.Pending, // "Em Revisão"
                    RequestedById = requestedById,
                    MainLink = dto.MainLink,
                    SecundaryLink = dto.SecundaryLink,
                    Obs = dto.Obs,
                    PublishedChannel = dto.PublishedChannel,
                    PublishedDate = dto.PublishedDate
                };

                // Associa a verificação à notícia e salva no banco
                await _db.Verifications.AddAsync(verification, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good("Verification created successfully.");
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

        public async Task<AppResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
                if (news == null)
                    return result.Bad("News not found");


                news.IsDeleted = true;

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

        public async Task<AppResult> GetAll(int page = 1, int take = 30, string filter = null, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var query = _db.News
                    .Include(n => n.Verification)
                    .Include(n => n.Tag)
                    .Include(n => n.PublishedBy)
                    .Where(x => !x.IsDeleted)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query = query.Where(x => x.Title.Contains(filter) || x.Resume.Contains(filter) || x.Text.Contains(filter));
                }

                var newsList = await query
                    .OrderByDescending(x => x.PublicationDate)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .Select(x => new NewsDTOOutput(x))
                    .ToListAsync(cancellationToken);

                return result.Good(newsList);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> GetAllPublished(int page = 1, int take = 30, string filter = null, CancellationToken cancellation = default)
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

                var publishedNews = await query
                    .OrderByDescending(x => x.PublicationDate)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToListAsync(cancellation);

                return result.Good(publishedNews);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }


        public async Task<AppResult> Like(LikeDTO dto, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News.FirstOrDefaultAsync(n => n.Id == dto.NewsId, cancellationToken);
                if (news == null)
                    return result.Bad("News not found");

                news.Like += 1;
                _db.News.Update(news);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good(news.Like);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> Unlike(UnlikeDTO dto, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var news = await _db.News.FirstOrDefaultAsync(n => n.Id == dto.NewsId, cancellationToken);
                if (news == null)
                    return result.Bad("News not found");

                news.UnLike += 1;
                _db.News.Update(news);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good(news.UnLike);
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
        public async Task<AppResult> GetNewsByTag(string tag, int page = 1, int take = 30, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var newsList = await _db.News
                    .Include(n => n.Tag)
                    .Where(n => n.Tag.Name.Equals(tag, StringComparison.OrdinalIgnoreCase) && !n.IsDeleted)
                    .OrderByDescending(n => n.PublicationDate)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToListAsync(cancellationToken);

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
        public async Task<AppResult> GetRecentlyVerifiedPublished(int page = 1, int take = 30, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var newsList = _db.News
                    .Include(n => n.Verification)
                    .Where(n => n.IsPublished &&
                                n.Verification.VerificationStatusId != (int)VerificationStatusEnum.InReview &&
                                !n.IsDeleted) // Assuming 1 is "Verified"
                    .OrderByDescending(n => n.Verification.VerificationDate)
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToListAsync(cancellationToken);

                return result.Good(newsList);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> GetPopularNews(int page = 1, int take = 30, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var newsList = await _db.News
                    .Where(n => n.IsPublished && !n.IsDeleted)
                    .OrderByDescending(n => n.Like - n.UnLike) // Assuming popularity is based on likes minus unlikes
                    .Skip((page - 1) * take)
                    .Take(take)
                    .ToListAsync(cancellationToken);

                return result.Good(newsList);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }
    }
}
