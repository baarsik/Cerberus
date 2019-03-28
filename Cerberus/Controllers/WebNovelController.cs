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

        public WebNovelController(WebNovelService webNovelService)
        {
            _webNovelService = webNovelService;
        }

        [Route("{page:int=1}", Order = -1)]
        public async Task<IActionResult> Index(int? page)
        {
            var user = await _webNovelService.GetUserAsync(User);
            var model = await _webNovelService.GetWebNovelIndexViewModelAsync(user, page ?? 1);
            return View(model);
        }

        [Route("[action]/{webNovelUrl}")]
        public async Task<IActionResult> Details(string webNovelUrl)
        {
            var user = await _webNovelService.GetUserAsync(User);
            var model = await _webNovelService.GetWebNovelDetailsViewModelAsync(user, webNovelUrl);
            return View(model);
        }
        
        [Route("[action]/{languageCode}/{webNovelUrl}/{chapterNumber:int}")]
        public async Task<IActionResult> Read(string webNovelUrl, string languageCode, int chapterNumber)
        {
            return await Read(webNovelUrl, languageCode, 1, chapterNumber);
        }
        
        [Route("[action]/{languageCode}/{webNovelUrl}/{volume:int}/{chapterNumber:int}")]
        public async Task<IActionResult> Read(string webNovelUrl, string languageCode, int volume, int chapterNumber)
        {
            var user = await _webNovelService.GetUserAsync(User);
            var model = await _webNovelService.GetChapterTranslationAsync(user, webNovelUrl, languageCode, volume, chapterNumber);
            
            if (model == null)
                return RedirectToNotFound();
            
            return View(model);
        }
        
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]")]
        public async Task<IActionResult> AddWebNovel()
        {
            var model = new AddWebNovelViewModel
            {
                Languages = await _webNovelService.GetLanguagesAsync()
            };
            return View(model);
        }
        
        [HttpPost]
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]")]
        public async Task<IActionResult> AddWebNovel(AddWebNovelViewModel model)
        {
            model.Languages = await _webNovelService.GetLanguagesAsync();
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _webNovelService.GetUserAsync(User);
            var result = await _webNovelService.AddWebNovelAsync(user, model);
            switch (result)
            {
                case WebNovelAddWebNovelResult.Success:
                    return RedirectToAction(nameof(Details), new { webNovelUrl = model.UrlName });
                case WebNovelAddWebNovelResult.WebNovelUrlExists:
                    ModelState.AddModelError(string.Empty, "Web novel already exists");
                    return View(model);
                case WebNovelAddWebNovelResult.LanguageNotExists:
                    ModelState.AddModelError(string.Empty, $"Language not found ({model.LanguageId})");
                    return View(model);
                case WebNovelAddWebNovelResult.UnknownFailure:
                default:
                    ModelState.AddModelError(string.Empty, $"Unknown error (Result code {result.ToString()})");
                    return View(model);
            }
        }
        
        [HttpGet]
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{webNovelUrl}")]
        public async Task<IActionResult> AddWebNovelTranslation(string webNovelUrl)
        {
            var user = await _webNovelService.GetUserAsync(User);
            var model = await _webNovelService.GetAddWebNovelViewModelAsync(user, webNovelUrl);
            
            if (model == null)
                return RedirectToNotFound();
            
            return View(model);
        }
        
        [HttpPost]
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{webNovelUrl}")]
        public async Task<IActionResult> AddWebNovelTranslation(AddWebNovelViewModel model)
        {
            var user = await _webNovelService.GetUserAsync(User);
            var baseModel = await _webNovelService.GetAddWebNovelViewModelAsync(user, model.UrlName);
            model.Languages = baseModel.Languages;
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _webNovelService.AddWebNovelTranslationAsync(model);
            switch (result)
            {
                case WebNovelAddWebNovelTranslationResult.Success:
                    return RedirectToAction(nameof(Details), new { webNovelUrl = model.UrlName });
                case WebNovelAddWebNovelTranslationResult.WebNovelNotExists:
                    ModelState.AddModelError(string.Empty, "Parent web novel was not found");
                    return View(model);
                case WebNovelAddWebNovelTranslationResult.LanguageNotExists:
                    ModelState.AddModelError(string.Empty, $"Language not found ({model.LanguageId})");
                    return View(model);
                case WebNovelAddWebNovelTranslationResult.TranslationExists:
                    ModelState.AddModelError(string.Empty, "Translation for selected language already exists");
                    return View(model);
                case WebNovelAddWebNovelTranslationResult.UnknownFailure:
                default:
                    ModelState.AddModelError(string.Empty, $"Unknown error (Result code {result.ToString()})");
                    return View(model);
            }
        }
        
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{webNovelId}")]
        public async Task<IActionResult> AddChapter(Guid webNovelId)
        {
            var user = await _webNovelService.GetUserAsync(User);
            var model = await _webNovelService.GetWebNovelAddChapterViewModelAsync(user, webNovelId);

            if (model.WebNovel.IsComplete)
                return View("IsCompleteError", model.WebNovel);
            
            return View(model);
        }
        
        [HttpPost]
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{webNovelId}")]
        public async Task<IActionResult> AddChapter(AddChapterViewModel model)
        {
            var user = await _webNovelService.GetUserAsync(User);
            var spoofModel = await _webNovelService.GetWebNovelAddChapterViewModelAsync(user, model.WebNovelId);
            model.WebNovel = spoofModel.WebNovel;
            model.WebNovelContent = spoofModel.WebNovelContent;
            model.Languages = spoofModel.Languages;
            
            if (model.WebNovel.IsComplete)
                return View("IsCompleteError", model.WebNovel);
            
            if (!ModelState.IsValid)
                return View(model);

            var result = await _webNovelService.AddChapterAsync(user, model);
            switch (result)
            {
                case WebNovelAddChapterResult.Success:
                    return RedirectToAction(nameof(Details), new { webNovelUrl = model.WebNovel.UrlName });
                case WebNovelAddChapterResult.TranslatedChapterNumberExists:
                    ModelState.AddModelError(string.Empty, $"Translation for chapter number {model.Number} already exists");
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