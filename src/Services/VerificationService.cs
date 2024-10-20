using Data.Context;
using Microsoft.EntityFrameworkCore;
using Services.Models.DTO;
using Services.Helpers;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.NewsVerfication;
using Services.Enums;
using Services.Models;
namespace Services
{
    public interface IVerificationService
    {
        Task<AppResult> CreateVerification(CreateOrEditVerificationDTO verificationDTO, Guid requestedById, CancellationToken cancellationToken = default);
        Task<AppResult> GetAllVerifications(PaginationParameters pagination, Guid? verifiedById = null, int statusId = 0, CancellationToken cancellationToken = default);
        Task<AppResult> ChangeVerificationStatus(ChangeVerificationStatusDTO changeStatusDTO, CancellationToken cancellationToken = default);
    }

    public class VerificationService : IVerificationService
    {
        private readonly DataContext _db;

        public VerificationService(DataContext db)
        {
            _db = db;
        }

        public async Task<AppResult> CreateVerification(CreateOrEditVerificationDTO dto, Guid requestedById, CancellationToken cancellationToken)
        {
            var result = new AppResult();
            try
            {
                var verification = new Verification
                {
                    PublishedTitle = dto.PublishedTitle,
                    VerificationStatusId = (int)VerificationStatusEnum.Pending,
                    RequestedById = requestedById,
                    MainLink = dto.MainLink,
                    SecundaryLink = dto.SecundaryLink,
                    Obs = dto.Obs,
                    PublishedChannel = dto.PublishedChannel,
                    PublishedDate = dto.PublishedDate,
                    VerifiedById = dto.VerifiedById,
                };

                await _db.Verifications.AddAsync(verification, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good("Verification created successfully.");
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> GetAllVerifications(PaginationParameters pagination, Guid? verifiedById = null, int statusId = 0, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var query = _db.Verifications
                    .Include(v => v.News)
                    .Include(v => v.VerificationStatus)
                    .Include(v => v.VerificationClassification)
                    .Include(v => v.RequestedBy)
                    .Include(v => v.VerifiedBy)
                    .AsQueryable();

                if (verifiedById != null)
                    query = query.Where(v => v.VerifiedById == verifiedById);

                if (statusId != 0)
                    query = query.Where(v => v.VerificationStatusId == statusId);

                var verifications = await query
                    .OrderByDescending(v => v.CreatedAt)
                    .Select(v => new VerificationDTOOutput
                    {
                        Id = v.Id,
                        VerificationStatusId = v.VerificationStatusId,
                        VerificationStatus = v.VerificationStatus.Name,
                        VerificationClassification = v.VerificationClassification.Name,
                        RequestedBy = v.RequestedBy.GetFullName(),
                        RequestedDate = v.CreatedAt,
                        MainLink = v.MainLink,
                        SecundaryLink = v.SecundaryLink,
                        PublishedDate = v.PublishedDate,
                        PublishedTile = v.PublishedTitle,
                        PublishedChannel = v.PublishedChannel,
                        Obs = v.Obs,
                        VerifiedByName = v.VerifiedBy == null ? "" : v.VerifiedBy.GetFullName(),
                    })
                    .ToPagedList(pagination.page, pagination.take, cancellationToken);

                return result.Good(verifications);
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }

        public async Task<AppResult> ChangeVerificationStatus(ChangeVerificationStatusDTO changeStatusDTO, CancellationToken cancellationToken = default)
        {
            var result = new AppResult();
            try
            {
                var verification = await _db.Verifications
                    .FirstOrDefaultAsync(x => x.Id == changeStatusDTO.VerificationId, cancellationToken);

                if (verification == null)
                    return result.Bad("Verificação inexistente.");

                verification.VerificationStatusId = changeStatusDTO.VerficationStatusId;
                verification.VerificationDate = DateTime.UtcNow;

                _db.Verifications.Update(verification);
                await _db.SaveChangesAsync(cancellationToken);

                return result.Good("Estado alterado com sucesso");
            }
            catch (Exception e)
            {
                return result.Bad(e.Message);
            }
        }
    }
}