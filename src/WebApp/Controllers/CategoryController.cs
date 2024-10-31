using System.Net.Security;

using Application.Areas.V1.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models.DTO;
using System;
using System.Threading.Tasks;
using Services.Models;

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
        [HttpPost("{categoryId}/user/{userId}")]
        public async Task<IActionResult> AssociateUserWithCategory([FromRoute] int categoryId, [FromRoute] Guid userId)
        {
            var result = await _categoryService.AssociateUserWithCategory(new AssociateUserCategoryDTO
            {
                CategoryId = categoryId,
                UserId = userId
            });
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        // Adicionar tag a um item
        [HttpDelete("{categoryId}/user/{userId}")]
        public async Task<IActionResult> DeassociateUserWithCategory([FromRoute] int categoryId, [FromRoute] Guid userId)
        {
            var result = await _categoryService.DeassociateUserWithCategory(new AssociateUserCategoryDTO
            {
                CategoryId = categoryId,
                UserId = userId
            });
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
        // Listar todas as tags
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQeury] PaginationFilterParameters page)
        {
            var result = await _categoryService.GetAllCategories(page.page, page.take, page.filter);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Listar tags ativas
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCategories(int page = 1, int take = 10, string? filter = "")
        {
            var result = await _categoryService.GetActiveCategories();
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
