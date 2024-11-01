using Services;
using Microsoft.AspNetCore.Mvc;
using Services.Models.DTO;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Services.Models;

namespace Application.Areas.V1.Controllers
{
    [ApiController]
    public class NewsController : BaseController
    {
        private readonly INewsService _newsService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NewsController(INewsService newsService, IHttpContextAccessor httpContextAccessor)
        {
            _newsService = newsService;
            _httpContextAccessor = httpContextAccessor;
        }

        // 1. Criar uma nova notícia
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNews([FromForm] CreateOrUpdateNewsDTO newsDTO, 
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Upload do ficheiro de capa
            if (newsDTO.CoverFile != null)
            {
                newsDTO.CoverUrl = UploadFile(newsDTO.CoverFile, "news", Guid.NewGuid());
            }

            var result = await _newsService.CreateNews(newsDTO, Guid.NewGuid(), cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 2. Editar uma notícia existente
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditNews(int id, [FromForm] CreateOrUpdateNewsDTO newsDTO, 
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Upload do ficheiro de capa
            if (newsDTO.CoverFile != null)
            {
                newsDTO.CoverUrl = UploadFile(newsDTO.CoverFile, "news", Guid.NewGuid());
            }

            var result = await _newsService.EditNews(id, newsDTO, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 3. Excluir uma notícia
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteNews(int id, CancellationToken cancellationToken)
        {
            var result = await _newsService.DeleteNews(id, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 4. Listar todas as notícias (paginadas)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNews([FromQuery] PaginationFilterParameters pagination, 
            CancellationToken cancellationToken = default)
        {
            var result = await _newsService.GetAllNews(pagination, false, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 5. Listar apenas notícias publicadas (paginadas)
        [HttpGet("published")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPublished([FromQuery] PaginationFilterParameters pagination, 
            CancellationToken cancellationToken = default)
        {
            var result = await _newsService.GetAllNews(pagination, true, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 6. Listar notícias populares
        [HttpGet("popular")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPopularNews([FromQuery] PaginationParameters pagination,
            CancellationToken cancellationToken = default)
        {
            var result = await _newsService.GetPopularNews(pagination, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 7. Listar notícias por categoria (paginadas)
        [HttpGet("category/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewsByCategorySlug(string slug, 
            [FromQuery] PaginationFilterParameters pagination, CancellationToken cancellationToken = default)
        {
            var result = await _newsService.GetNewsByCategorySlug(slug, pagination, true, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewsDetailsById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _newsService.GetNewsDetailsById(id, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewsDetailsBySlug(string slug, 
            CancellationToken cancellationToken = default)
        {
            var result = await _newsService.GetNewsDetailsBySlug(slug, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        // 9. Gostar de uma notícia
        [HttpPost("like")]
        [Authorize]
        public async Task<IActionResult> Like([FromBody] LikeDTO dto, CancellationToken cancellationToken)
        {
            dto.SetUserId(GetUserId());
            var result = await _newsService.LikeNews(dto, cancellationToken);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // 10. Desgostar de uma notícia
        [HttpPost("unlike")]
        [Authorize]
        public async Task<IActionResult> Unlike([FromBody] UnlikeDTO dto, CancellationToken cancellationToken)
        {
            dto.SetUserId(GetUserId());
            var result = await _newsService.UnlikeNews(dto, cancellationToken);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        // 11. Alterar o status de publicação
        [HttpPatch("toggle-publish/{id}")]
        [Authorize]
        public async Task<IActionResult> TogglePublishStatus(int id, CancellationToken cancellationToken)
        {
            var result = await _newsService.TogglePublishStatus(id, cancellationToken);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
