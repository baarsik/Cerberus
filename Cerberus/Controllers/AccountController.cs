using System;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Cerberus.Models;
using Cerberus.Models.Extensions;
using Cerberus.Models.Services;
using Cerberus.Models.ViewModels;
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
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
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

        public async Task<IActionResult> Register()
        {
            var model = new RegisterViewModel
            {
                Languages = await _authService.GetLanguagesAsync()
            };
            return View(model);
        }

        [ValidateRecaptcha]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            model.Languages = await _authService.GetLanguagesAsync();
            
            if (!ModelState.IsValid)
                return View(model);
            
            var result = await _authService.RegisterAsync(model.Email, model.DisplayName, model.Password, model.SelectedLanguages.ToList());
            
            switch (result.Status)
            {
                case RegisterStatus.Success:
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                case RegisterStatus.DisplayNameInUse:
                    ModelState.AddModelError("", "Display Name is already in use");
                    break;
                case RegisterStatus.EmailInUse:
                    ModelState.AddModelError("", "User with this E-Mail already exists");
                    break;
                case RegisterStatus.LanguageNotExists:
                    ModelState.AddModelError("", "One of selected language does not exist in the database");
                    break;
                case RegisterStatus.Failure:
                    ModelState.AddModelError("", "Please verify that your password meets following criteria: it has to be 8+ length, contain digit(s), lowercase letter(s) and uppercase letter(s) and have 4+ unique chars");
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
            return RedirectToAction(nameof(HomeController.Index), "Home");
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
            return RedirectToAction(nameof(ProfileController.Profile), "Profile", new {name = User.GetDisplayName()});
        }
    }
}
