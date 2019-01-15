using System.Collections.Generic;
using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Cerberus.Models;
using Cerberus.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cerberus.Controllers
{
    [Authorize(Roles = Constants.Roles.Admin)]
    public class SettingsController : BaseController
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