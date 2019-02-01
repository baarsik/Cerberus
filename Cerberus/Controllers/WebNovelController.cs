using System;
using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Cerberus.Models.ViewModels;
using DataContext.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers
{
    [Route("wn")]
    public class WebNovelController : BaseController
    {
        private readonly WebNovelService _webNovelService;
        private readonly UserManager<ApplicationUser> _userManager;

        public WebNovelController(WebNovelService webNovelService, UserManager<ApplicationUser> userManager)
        {
            _webNovelService = webNovelService;
            _userManager = userManager;
        }

        [Route("{page?}", Order = -1)]
        public async Task<IActionResult> Index(int? page)
        {
            var model = await _webNovelService.GetWebNovelIndexViewModelAsync(page ?? 1);
            return View(model);
        }

        [Route("[action]/{webNovelUrl}")]
        public async Task<IActionResult> Details(string webNovelUrl)
        {
            var model = await _webNovelService.GetWebNovelDetailsViewModelAsync(webNovelUrl);
            return View(model);
        }
        
        [Route("[action]/{webNovelUrl}/{chapterNumber}")]
        public IActionResult Read(string webNovelUrl, string chapterNumber)
        {
            throw new NotImplementedException();
        }
        
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]")]
        public IActionResult AddWebNovel()
        {
            throw new NotImplementedException();
        }
        
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{webNovelId}")]
        public async Task<IActionResult> AddChapter(Guid webNovelId)
        {
            var model = await _webNovelService.GetWebNovelAddChapterViewModelAsync(webNovelId);
            return View(model);
        }
        
        [HttpPost]
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{webNovelId}")]
        public async Task<IActionResult> AddChapter(AddChapterViewModel model)
        {
            model.WebNovel = await _webNovelService.GetWebNovelByIdAsync(model.WebNovelId);
            model.Languages = await _webNovelService.GetLanguagesAsync();
            
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            var result = await _webNovelService.AddChapterAsync(user, model);
            switch (result)
            {
                case WebNovelAddChapterResult.Success:
                    return RedirectToAction(nameof(Details), new { webNovelUrl = model.WebNovel.UrlName });
                case WebNovelAddChapterResult.NumberExists:
                    ModelState.AddModelError(string.Empty, $"Chapter number {model.Number} already exists");
                    return View(model);
                case WebNovelAddChapterResult.WebNovelNotExists:
                    ModelState.AddModelError(string.Empty, "Parent web novel was not found");
                    return View(model);
                case WebNovelAddChapterResult.LanguageNotExists:
                    ModelState.AddModelError(string.Empty, $"Language not found ({model.LanguageId})");
                    return View(model);
                case WebNovelAddChapterResult.UnknownFailure:
                default:
                    ModelState.AddModelError(string.Empty, $"Unknown error (Result code {result.ToString()})");
                    return View(model);
            }
        }
    }
}