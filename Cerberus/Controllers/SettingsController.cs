using Cerberus.Controllers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers
{
    [Authorize(Roles = "admin")]
    public class SettingsController : Controller
    {
        private readonly SettingsService _settingsService;

        public SettingsController(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Forums()
        {
            var model = _settingsService.GetForumSettingsViewModel();
            return View(model);
        }
    }
}