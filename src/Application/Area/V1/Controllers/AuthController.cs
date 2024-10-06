using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;
using Services.Models.DTO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
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
          [HttpPost("recover-password")]
        public async Task<ActionResult<AppResult>> RecoverPassword([FromBody] RecoveryPasswordDTO recoveryDto)
        {
            var result = await _userService.RecoverPassword(recoveryDto);
            if (result.Success)
            {
                return Ok(result); 
            }

            return BadRequest(result); 
        }
        [HttpPost("request-update-password")]
        public async Task<ActionResult<AppResult>> RequestUpdatePassword([FromBody] RequestVerifyEmailDTO requestVerifyEmailDTO)
        {
            var result = await _userService.RequestUpdatePassword(requestVerifyEmailDTO);
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
