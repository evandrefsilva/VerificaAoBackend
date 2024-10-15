using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Models.DTO;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Application.Areas.V1.Controllers
{
    [ApiController]
    public class VerificationController : BaseController
    {
        private readonly INewsService _newsService;

        public VerificationController(INewsService newsService)
        {
            _newsService = newsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllVerifications(
            Guid? verifiedById = null, int statusId = 0,
            int page = 1, int take = 30)
        {
            var result = await _newsService.GetAllVerfications(page, take, verifiedById, statusId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 4. Verificar uma notícia
        [HttpPost("request")]
        //[Authorize]
        public async Task<IActionResult> RequestVerification([FromForm] VerificationDTOInput verificationDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _newsService.CreateVerification(verificationDTO, Guid.Parse(Guid.Empty.ToString().Replace("0", "1")), cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPatch("status")]
        //[Authorize]
        public async Task<IActionResult> ChangeVerificationStatus
            ([FromForm] ChangeVerificationStatusDTO dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _newsService.ChangeVerificationStatus(dto, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
