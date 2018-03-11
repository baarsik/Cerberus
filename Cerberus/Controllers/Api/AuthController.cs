using System.Threading.Tasks;
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
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _authService.GetUserByCredentials(email, password);
            if (user == null)
                return new UnauthorizedResult();
            
            var token = _authService.GenerateJwtToken(email, user);
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