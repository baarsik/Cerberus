using System;
using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Cerberus.Models;
using Cerberus.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

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

        [ValidateRecaptcha]
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

        [ValidateRecaptcha]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var result = await _authService.JwtRegisterAsync(model.Email, model.Login, model.Password);
            
            switch (result.Status)
            {
                case RegisterStatus.Success:
                    return RedirectToAction("Index", "Home");
                case RegisterStatus.DisplayNameInUse:
                    ModelState.AddModelError("", "Display Name is already in use");
                    break;
                case RegisterStatus.EmailInUse:
                    ModelState.AddModelError("", "User with this E-Mail already exists");
                    break;
                case RegisterStatus.Failure:
                    ModelState.AddModelError("", "Unknown failure");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return View(model);
        }
        
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
