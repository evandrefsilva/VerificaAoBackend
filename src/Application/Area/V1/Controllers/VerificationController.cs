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
        public async Task<IActionResult> GetAllVerifications(int page = 1, int take = 30, string filter = null)
        {
            var result = await _newsService.GetAllVerfications(page, take);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 4. Verificar uma notícia
        [HttpPost("request")]
        //[Authorize]
        public async Task<IActionResult> VerifyNews([FromForm] VerificationDTOInput verificationDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _newsService.CreateVerification(verificationDTO, Guid.Parse(Guid.Empty.ToString().Replace("0", "1")), cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }


    }
}
