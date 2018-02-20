using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Cerberus.Models;
using Cerberus.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers
{
    public class AccountController : Controller
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
            var result = await _authService.JwtLoginAsync(model.Login, model.Password, model.RememberMe);

            return result.Status == LoginStatus.SUCCESS
                ? RedirectToLocal(returnUrl)
                : View(model);;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var result = await _authService.JwtRegisterAsync(model.Email, model.Login, model.Password);
            return result.Status == RegisterStatus.SUCCESS
                ? (IActionResult)RedirectToAction("Index", "Home")
                : View(model); ;
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            return Url.IsLocalUrl(returnUrl)
                ? (IActionResult)Redirect(returnUrl)
                : RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
