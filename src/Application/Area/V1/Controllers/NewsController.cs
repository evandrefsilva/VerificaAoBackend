using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Models.DTO;
using System.Threading.Tasks;
using System.Threading;

namespace Application.Areas.V1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewsController : BaseController
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // 1. Criar uma nova notícia
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateNews([FromBody] NewsDTOInput newsDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _newsService.Create(newsDTO, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 2. Editar uma notícia existente
        [HttpPut("{id}/edit")]
        [Authorize]
        public async Task<IActionResult> EditNews(int id, [FromBody] NewsDTOInput newsDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _newsService.Edit(id, newsDTO, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 3. Listar todas as notícias
        [HttpGet("all")]
        [AllowAnonymous]
        public IActionResult GetAllNews(int page = 1, int take = 30, string filter = null)
        {
            var result = _newsService.GetAll(page, take, filter);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 4. Verificar uma notícia
        [HttpPost("{newsId}/verify")]
        [Authorize]
        public async Task<IActionResult> VerifyNews(int newsId, [FromBody] VerificationDTOInput verificationDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _newsService.VerifyNews(newsId, verificationDTO, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 5. Listar notícias publicadas
        [HttpGet("published")]
        [AllowAnonymous]
        public IActionResult GetAllPublished(int page = 1, int take = 30, string filter = null)
        {
            var result = _newsService.GetAllPublished(page, take, filter);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 6. Listar notícias populares
        [HttpGet("popular")]
        [AllowAnonymous]
        public IActionResult GetPopularNews(int page = 1, int take = 30)
        {
            var result = _newsService.GetPopularNews(page, take);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 7. Listar notícias por tag
        [HttpGet("tag/{tag}")]
        [AllowAnonymous]
        public IActionResult GetNewsByTag(string tag, int page = 1, int take = 30)
        {
            var result = _newsService.GetNewsByTag(tag, page, take);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
