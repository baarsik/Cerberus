using System.Threading.Tasks;
using Cerberus.Controllers.Services;
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
            var ip = HttpContext.Connection.RemoteIpAddress.ToString();
            var result = await _authService.GenerateJwtAsync(email, apiKey, ip);
            
            if (!result.Success)
                return new UnauthorizedResult();
            
            return new OkObjectResult(new
            {
                email = result.User.Email,
                name = result.User.DisplayName,
                result.Token
            });
        }
    }
}