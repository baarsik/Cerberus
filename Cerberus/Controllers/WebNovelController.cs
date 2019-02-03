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

        [Route("{page:int=1}", Order = -1)]
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
        
        [Route("[action]/{webNovelUrl}/{chapterNumber:int}")]
        public async Task<IActionResult> Read(string webNovelUrl, int chapterNumber)
        {
            return await Read(webNovelUrl, 1, chapterNumber);
        }
        
        [Route("[action]/{webNovelUrl}/{volume:int}/{chapterNumber:int}")]
        public async Task<IActionResult> Read(string webNovelUrl, int volume, int chapterNumber)
        {
            var model = await _webNovelService.GetChapterAsync(webNovelUrl, volume, chapterNumber);
            
            if (model == null)
                return RedirectToNotFound();
            
            return View(model);
        }
        
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]")]
        public IActionResult AddWebNovel()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]")]
        public async Task<IActionResult> AddWebNovel(AddWebNovelViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var user = await _userManager.GetUserAsync(User);
            var result = await _webNovelService.AddWebNovelAsync(user, model);
            switch (result)
            {
                case WebNovelAddWebNovelResult.Success:
                    return RedirectToAction(nameof(Details), new { webNovelUrl = model.UrlName });
                case WebNovelAddWebNovelResult.WebNovelUrlExists:
                    ModelState.AddModelError(string.Empty, "Parent web novel was not found");
                    return View(model);
                case WebNovelAddWebNovelResult.UnknownFailure:
                default:
                    ModelState.AddModelError(string.Empty, $"Unknown error (Result code {result.ToString()})");
                    return View(model);
            }
        }
        
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{webNovelId}")]
        public async Task<IActionResult> AddChapter(Guid webNovelId)
        {
            var model = await _webNovelService.GetWebNovelAddChapterViewModelAsync(webNovelId);

            if (model.WebNovel.IsComplete)
                return View("IsCompleteError", model.WebNovel);
            
            return View(model);
        }
        
        [HttpPost]
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{webNovelId}")]
        public async Task<IActionResult> AddChapter(AddChapterViewModel model)
        {
            model.WebNovel = await _webNovelService.GetWebNovelByIdAsync(model.WebNovelId);
            model.Languages = await _webNovelService.GetLanguagesAsync();
            
            if (model.WebNovel.IsComplete)
                return View("IsCompleteError", model.WebNovel);
            
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