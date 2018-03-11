using System;
using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Cerberus.Models;
using Cerberus.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers
{
    public class AccountController : BaseController
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var result = await _authService.JwtLoginAsync(model.Email, model.Password, model.RememberMe);

            switch (result.Status)
            {
                case LoginStatus.Success:
                    return RedirectToLocal(returnUrl);
                case LoginStatus.InvalidCredentials:
                    ModelState.AddModelError("", "Invalid E-Mail or password");
                    break;
                case LoginStatus.AccountLocked:
                    ModelState.AddModelError("", "Your account is banned");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _authService.JwtRegisterAsync(model.Email, model.Login, model.Password);
            return result.Status == RegisterStatus.Success
                ? (IActionResult)RedirectToAction("Index", "Home")
                : View(model);
        }
        
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
