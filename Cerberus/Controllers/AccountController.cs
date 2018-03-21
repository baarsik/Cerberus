using System;
using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Cerberus.Models;
using Cerberus.Models.Extensions;
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
            
            var result = await _authService.LoginAsync(model.Email, model.Password, model.RememberMe);

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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var result = await _authService.RegisterAsync(model.Email, model.DisplayName, model.Password);
            
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

        [Authorize]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _authService.GetUserByClaimsPrincipalAsync(User);
            if (user == null)
                return new UnauthorizedResult();

            var model = new ChangePasswordViewModel
            {
                User = user
            };
            return View(model);
        }
        
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _authService.GetUserByDisplayNameAsync(User.GetDisplayName(), model.OldPassword);
            if (user == null)
            {
                ModelState.AddModelError("", "Wrong old password");
                return View(model);
            }

            await _authService.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return RedirectToAction("Profile", "Profile", new {name = User.GetDisplayName()});
        }
    }
}
