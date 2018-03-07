﻿using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Cerberus.Models;
using Cerberus.Models.Services;
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
            var result = await _authService.JwtLoginAsync(model.Email, model.Password, model.RememberMe);

            return result.Status == LoginStatus.Success
                ? RedirectToLocal(returnUrl)
                : View(model);
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
    }
}
