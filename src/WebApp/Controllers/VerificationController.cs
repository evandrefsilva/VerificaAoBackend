using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Models.DTO;
using System.Threading.Tasks;
using System.Threading;
using System;
using Services.Models;

namespace Application.Areas.V1.Controllers
{
    [ApiController]
    public class VerificationController : BaseController
    {
        private readonly IVerificationService _verificationService;

        public VerificationController(IVerificationService verificationService)
        {
            _verificationService = verificationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllVerifications(
            [FromQuery] PaginationParameters page,
            Guid? verifiedById = null, int statusId = 0)
        {
            var result = await _verificationService.GetAllVerifications(page, verifiedById, statusId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 4. Verificar uma notícia
        [HttpPost("request")]
        //[Authorize]
        public async Task<IActionResult> RequestVerification([FromForm] CreateOrEditVerificationDTO verificationDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _verificationService.CreateVerification(verificationDTO, Guid.Parse(Guid.Empty.ToString().Replace("0", "1")), cancellationToken);
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

            var result = await _verificationService.ChangeVerificationStatus(dto, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
