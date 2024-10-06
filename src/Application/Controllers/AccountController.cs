using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                RedirectToAction("index", "admin");

            return PartialView();
        }
        [HttpPost]
        public IActionResult Login(LoginDTO dto)
        {
            var resultLogin = _userService.Login(dto);
            if (resultLogin.Success)
            {
                var user = (ReturnLoginDTO)resultLogin.Object;
                var name = user.Name;

                var userId = user.UserId.ToString().ToLower();
                var email = user.Email;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,dto.Email),
                    new Claim("name", name),
                    new Claim("email", email),
                    new Claim(ClaimTypes.Role, "admin"),

                    new Claim(ClaimTypes.PrimarySid,userId),
              };

                var claimsIdentity = new ClaimsIdentity(claims, "Login");

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties { IsPersistent = true });

                return RedirectToAction("index", "admin");
            }
            else
            {
                return RedirectToAction("login", "account");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("login", "account");
        }
    }
}
