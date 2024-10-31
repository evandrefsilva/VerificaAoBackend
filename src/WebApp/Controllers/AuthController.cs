using Application.Areas.V1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;
using Services.Models.DTO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public ActionResult<AppResult> Login([FromBody] LoginDTO loginDto)
        {
            var result = _userService.Login(loginDto);
            if (result.Success)
            {
                return Ok(result); 
            }

            return BadRequest(result); 
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppResult>> Register([FromBody] RegisterUserDTO registerDto)
        {
            var result = await _userService.Register(registerDto);
            if (result.Success)
            {
                return Ok(result); 
            }

            return BadRequest(result); 
        }
          [HttpPost("recovery-password")]
        public async Task<ActionResult<AppResult>> RecoverPassword([FromBody] RecoveryPasswordDTO recoveryDto)
        {
            var result = await _userService.RecoverPassword(recoveryDto);
            if (result.Success)
            {
                return Ok(result); 
            }

            return BadRequest(result); 
        }
        [HttpPost("update-password")]
        public async Task<ActionResult<AppResult>> UpdatePassword([FromBody] UpdatePasswordDTO updatePasswordDTO)
        {
            var result = await _userService.UpdatePassword(updatePasswordDTO);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpPost("request-verify-email")]
        public async Task<ActionResult<AppResult>> VerifyEmail([FromBody] RequestVerifyEmailDTO requestVerifyEmailDTO)
        {
            var result = await _userService.RequestVerifyEmail(requestVerifyEmailDTO);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpPost("verify-email")]
        public async Task<ActionResult<AppResult>> VerifyEmail([FromBody] VerifyEmailDTO verifyDto)
        {
            var result = await _userService.VerifyEmail(verifyDto);
            if (result.Success)
            {
                return Ok(result); 
            }

            return BadRequest(result); 
        }
    }
}
