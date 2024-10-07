
using Application.Areas.V1.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models.DTO;
using System.Threading.Tasks;

namespace Application.Area.V1.Controllers
{
    [ApiController]
    //[Authorize(Roles = "Admin, Manager")]
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // Criar uma nova tag
        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagDTO dto)
        {
            var result = await _tagService.CreateTag(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Atualizar uma tag
        [HttpPut]
        public async Task<IActionResult> UpdateTag([FromBody] UpdateTagDTO dto)
        {
            var result = await _tagService.UpdateTag(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Apagar uma tag
        [HttpDelete]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var result = await _tagService.DeleteTag(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        //// Adicionar tag a um item
        //[HttpPost("add-to-item")]
        //public async Task<IActionResult> AddTagToItem([FromBody] AddTagToItemDTO dto)
        //{
        //    var result = await _tagService.AddTagToItem(dto);
        //    if (!result.Success)
        //        return BadRequest(result.Message);

        //    return Ok(result.Message);
        //}

        // Listar todas as tags
        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            var result = await _tagService.GetAllTags();
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Listar tags ativas
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveTags()
        {
            var result = await _tagService.GetActiveTags();
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Tornar uma tag ativa ou inativa
        [HttpPut("toggle-status/{id}")]
        public async Task<IActionResult> ToggleTagStatus(int id)
        {
            var tagResul = await _tagService.GetTagById(id);
            if (!tagResul.Success)
                return BadRequest(tagResul);
            var tag = (TagDTO)tagResul.Object;
            var updateDto = new UpdateTagDTO
            {
                Id = id,
                Name = tag.Name,
                IsActive = !tag.IsActive
            };

            var result = await _tagService.UpdateTag(updateDto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
