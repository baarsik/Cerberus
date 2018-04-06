using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Cerberus.Models;
using Cerberus.Models.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AuthService _authService;
        private readonly ProfileService _profileService;

        public ProfileController(AuthService authService, ProfileService profileService)
        {
            _authService = authService;
            _profileService = profileService;
        }
        
        [Route("[controller]/{name}")]
        public async Task<IActionResult> Profile(string name)
        {
            var user = await _authService.GetUserByDisplayNameAsync(name);
            if (user == null)
                return new NotFoundResult();
            
            var model = new ProfileViewModel
            {
                IsOwnProfile = User.GetDisplayName() == user.DisplayName,
                User = user,
                Statistics = await _profileService.GetUserStatisticsAsync(user)
            };
            return View(model);
        }

        [Route("[controller]/[action]")]
        public async Task<IActionResult> RegenerateApiKey()
        {
            var user = await _authService.GetUserByClaimsPrincipalAsync(User);
            if (user == null)
                return new UnauthorizedResult();

            await _authService.RegenerateApiKey(user);
            return RedirectToAction("Profile", "Profile", new { name = user.DisplayName });
        }
    }
}