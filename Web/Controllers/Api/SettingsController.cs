using System.Threading.Tasks;
using Web.Controllers.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    public class SettingsController : Controller
    {
        private readonly AuthService _authService;

        public SettingsController(AuthService authService)
        {
            _authService = authService;
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Test()
        {
            if (!await _authService.IsApiBindedIpValidAsync(User, HttpContext))
                return new UnauthorizedResult();
            
            return new OkObjectResult("It works!");
        }
    }
}