using System.Net.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models.DTO;
using System.Threading.Tasks;
using Services.Models;

namespace Application.Areas.V1.Controllers
{

    [ApiController]
    //[Authorize(Roles = "Admin, Manager")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Listar todos os utilizadores
        [HttpGet]
        public async Task<IActionResult> ListUsers([FromQuery] PaginationParameters page)
        {
            
            var result = await _userService.ListUsers(page.page, page.take);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Criar novo utilizador
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreateUser(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Editar utilizador existente
        [HttpPut]
        public async Task<IActionResult> EditUser([FromBody] EditUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.EditUser(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Apagar utilizador
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Listar todos os perfis (roles)
        [HttpGet("permissions")]
        public async Task<IActionResult> ListPermissions()
        {
            var result = await _userService.ListPermissions();
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }


        // Listar todos os perfis (roles)
        [HttpGet("roles")]
        public async Task<IActionResult> ListRoles()
        {
            var result = await _userService.ListRoles();
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Criar novo perfil com permissões
        [HttpPost("roles")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.CreateRole(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Editar perfil e suas permissões
        [HttpPut("roles")]
        public async Task<IActionResult> EditRole([FromBody] EditRoleDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.EditRole(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        // Apagar perfil
        [HttpDelete("roles/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _userService.DeleteRole(id);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result.Message);
        }
    }
}
