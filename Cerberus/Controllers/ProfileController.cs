using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Cerberus.Models.Extensions;
using Cerberus.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers
{
    public class ProfileController : BaseController
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

            var isOwnProfile = User.GetDisplayName() == user.DisplayName;
            var model = new ProfileViewModel
            {
                IsOwnProfile = isOwnProfile,
                User = user,
                Statistics = await _profileService.GetUserStatisticsAsync(user),
                ProductLicenses = isOwnProfile ? await _profileService.GetProductLicensesAsync(user) : null
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
            return RedirectToAction(nameof(Profile), "Profile", new { name = user.DisplayName });
        }
    }
}