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
    public class NewsController : BaseController
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // 1. Criar uma nova notícia
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateNews([FromForm] NewsDTOInput newsDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (newsDTO.CoverFile != null)
                newsDTO.CoverUrl = UploadFile(newsDTO.CoverFile, "news", Guid.NewGuid());

            var result = await _newsService.Create(newsDTO, Guid.Parse(Guid.Empty.ToString().Replace("0", "1")), cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 2. Editar uma notícia existente
        [HttpPut("{id}")]
        // [Authorize]
        public async Task<IActionResult> EditNews(int id, [FromForm] NewsDTOInput newsDTO, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (newsDTO.CoverFile != null)
                newsDTO.CoverUrl = UploadFile(newsDTO.CoverFile, "news", Guid.NewGuid());

            var result = await _newsService.Edit(id, newsDTO, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete("{id}")]
        // [Authorize]
        public async Task<IActionResult> DeleteNews(int id, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _newsService.Delete(id, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        // 3. Listar todas as notícias
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNews(int page = 1, int take = 30, string filter = null)
        {
            var result = await _newsService.GetAll(page, take, filter);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        // 5. Listar notícias publicadas
        [HttpGet("published")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPublished(int page = 1, int take = 30, string filter = null)
        {
            var result = await _newsService.GetAllPublished(page, take, filter);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        // 6. Listar notícias populares
        [HttpGet("popular")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPopularNews(int page = 1, int take = 30)
        {
            var result = await _newsService.GetPopularNews(page, take);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("tag/{tag}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewsByTag(string tag, int page = 1, int take = 30)
        {
            var result = await _newsService.GetNewsByTag(tag, page, take);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("tag/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewsById(int id, int page = 1, int take = 30, CancellationToken cancellationToken = default)
        {
            var result = await _newsService.GetNewsDetails(id, cancellationToken);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost("like")]
        public async Task<IActionResult> Like([FromBody] LikeDTO dto, CancellationToken cancellationToken)
        {
            var result = await _newsService.Like(dto, cancellationToken);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Endpoint for Unlike
        [HttpPost("unlike")]
        public async Task<IActionResult> Unlike([FromBody] UnlikeDTO dto, CancellationToken cancellationToken)
        {
            var result = await _newsService.Unlike(dto, cancellationToken);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpPatch("toogle-publish/{id}")]
        public async Task<IActionResult> TooglePusblish(int Id, CancellationToken cancellationToken)
        {
            var result = await _newsService.TooglePusblish(Id, cancellationToken);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
