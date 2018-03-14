﻿using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(string email, string apiKey)
        {
            var user = await _authService.GetUserByApiCredentialsAsync(email, apiKey);
            if (user == null)
                return new UnauthorizedResult();
            
            var token = _authService.GenerateJwt(user);
            return new OkObjectResult(new
            {
                email = user.Email,
                name = user.DisplayName,
                token
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Test()
        {
            return new OkObjectResult("It works!");
        }
    }
}