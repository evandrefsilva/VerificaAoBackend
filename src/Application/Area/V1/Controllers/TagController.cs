
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
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService tagService)
        {
            _categoryService = tagService;
        }

        // Criar uma nova tag
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDTO dto)
        {
            var result = await _categoryService.CreateCategory(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Atualizar uma tag
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDTO dto)
        {
            var result = await _categoryService.UpdateCategory(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Apagar uma tag
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategory(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Adicionar tag a um item
        [HttpPost("{categoryId}/users{userId}")]
        public async Task<IActionResult> AddCategoryToItem([FromBody] AssociateUserCategoryDTO dto)
        {
            var result = await _categoryService.AssociateUserWithCategory(dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
        // Adicionar tag a um item
        [HttpDelete("{categoryId}/users{userId}")]
        public async Task<IActionResult> RemoveCategoryToItem([FromBody] AssociateUserCategoryDTO dto)
        {
            var result = await _categoryService.AssociateUserWithCategory(dto);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
        // Listar todas as tags
        [HttpGet]
        public async Task<IActionResult> GetAllCategorys()
        {
            var result = await _categoryService.GetAllCategorys();
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Listar tags ativas
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCategorys()
        {
            var result = await _categoryService.GetActiveCategorys();
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Tornar uma tag ativa ou inativa
        [HttpPut("toggle-status/{id}")]
        public async Task<IActionResult> ToggleCategoryStatus(int id)
        {
            var tagResul = await _categoryService.GetCategoryById(id);
            if (!tagResul.Success)
                return BadRequest(tagResul);
            var tag = (CategoryDTO)tagResul.Object;
            var updateDto = new UpdateCategoryDTO
            {
                Id = id,
                Name = tag.Name,
                IsActive = !tag.IsActive
            };

            var result = await _categoryService.UpdateCategory(updateDto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
