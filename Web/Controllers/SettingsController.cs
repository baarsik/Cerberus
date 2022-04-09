using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Controllers.Services;
using Web.Models;
using Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web.Controllers
{
    [Authorize(Roles = Constants.Roles.Admin)]
    public class SettingsController : BaseController
    {
        private readonly SettingsService _settingsService;

        public SettingsController(SettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public IActionResult Forums()
        {
            var model = _settingsService.GetForumSettingsViewModel();
            return View("Forums/Index", model);
        }

        [Route("[controller]/forums/create")]
        public IActionResult CreateForum()
        {
            return View("Forums/Create");
        }
        
        [HttpPost]
        [Route("[controller]/forums/create")]
        public async Task<IActionResult> CreateForum(CreateForumViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Forums/Create", model);
            
            await _settingsService.CreateForum(model);
            return RedirectToAction(nameof(Forums));
        }

        [HttpPost]
        [Route("[controller]/forums/save")]
        public async Task<IActionResult> SaveForums(string forumHierarchy)
        {
            var forumHierarchyList = JsonConvert.DeserializeObject<ICollection<ForumHierarchyJson>>(forumHierarchy);
            await _settingsService.UpdateForums(forumHierarchyList);
            return RedirectToAction(nameof(Forums));
        }
    }
}