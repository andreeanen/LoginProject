using LoginProject.Models;
using LoginProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var token = await _accountService.GetToken(model);
                    if (string.IsNullOrEmpty(token))
                    {
                        ModelState.AddModelError("", "Authentication failed. The token was empty.");
                        return View(model);
                    }

                    await SetSignInUser(model, token);
                    var base64Image = await _accountService.GetImageString(token) ?? string.Empty;

                    var user = new UserViewModel
                    {
                        UserName = model.Username,
                        ImageBase64 = base64Image,
                    };

                    return RedirectToAction("Index", "Home", user);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);
                }
            }

            ModelState.AddModelError("", "Authentication failed");
            return View(model);
        }

        private async Task SetSignInUser(LoginViewModel model, string? token)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username),
                        new Claim("access_token", token)
                    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}
