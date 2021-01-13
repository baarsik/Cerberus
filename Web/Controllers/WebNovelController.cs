using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Controllers.Services;
using Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    
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

            if (model == null)
                return RedirectToNotFound();
            
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

            await _webNovelService.UpdateLastReadChapterAsync(user, model.Translation.Chapter);
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
                    ModelState.AddModelError(string.Empty, $"Unknown error (Result code {result})");
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
            
            return model == null
                ? RedirectToNotFound()
                : View(model);
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
                    ModelState.AddModelError(string.Empty, $"Unknown error (Result code {result})");
                    return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{translationId}")]
        public async Task<IActionResult> EditWebNovelTranslation(Guid translationId)
        {
            var model = await _webNovelService.GetEditWebNovelViewModelAsync(translationId);
            return model == null
                ? RedirectToNotFound()
                : View(model);
        }

        [HttpPost]
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{translationId}")]
        public async Task<IActionResult> EditWebNovelTranslation(EditWebNovelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _webNovelService.EditWebNovelTranslationAsync(model);
            switch (result)
            {
                case WebNovelEditWebNovelTranslationResult.Success:
                    return RedirectToAction(nameof(Details), new { webNovelUrl = model.UrlName });
                case WebNovelEditWebNovelTranslationResult.TranslationNotExists:
                    ModelState.AddModelError(string.Empty, $"Translation for '{model.LanguageName}' language does not exist");
                    return View(model);
                case WebNovelEditWebNovelTranslationResult.UnknownFailure:
                default:
                    ModelState.AddModelError(string.Empty, $"Unknown error (Result code {result})");
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
            var spoofModel = model.IsTranslation
                ? await _webNovelService.GetWebNovelAddChapterTranslationViewModelAsync(user, model.WebNovelId, model.Number)
                : await _webNovelService.GetWebNovelAddChapterViewModelAsync(user, model.WebNovelId);
            model.WebNovel = spoofModel.WebNovel;
            model.WebNovelContent = spoofModel.WebNovelContent;
            model.Languages = spoofModel.Languages;
            
            if (model.WebNovel.IsComplete)
                return View("IsCompleteError", model.WebNovel);
            
            if (!ModelState.IsValid)
                return View(model);

            var result = await _webNovelService.AddChapterContentAsync(user, model);
            switch (result)
            {
                case WebNovelAddChapterResult.Success:
                    return RedirectToAction(nameof(Details), new { webNovelUrl = model.WebNovel.UrlName });
                case WebNovelAddChapterResult.TranslatedChapterNumberExists:
                    var languageName = model.Languages
                        .Where(c => c.Id == model.LanguageId)
                        .Select(c => c.GlobalName)
                        .FirstOrDefault();
                    ModelState.AddModelError(string.Empty, $"{languageName} translation for chapter number {model.Number} already exists");
                    return View(model);
                case WebNovelAddChapterResult.WebNovelNotExists:
                    ModelState.AddModelError(string.Empty, "Parent web novel was not found");
                    return View(model);
                case WebNovelAddChapterResult.LanguageNotExists:
                    ModelState.AddModelError(string.Empty, $"Language not found ({model.LanguageId})");
                    return View(model);
                case WebNovelAddChapterResult.UnknownFailure:
                default:
                    ModelState.AddModelError(string.Empty, $"Unknown error (Result code {result})");
                    return View(model);
            }
        }
        
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{webNovelChapterContentId}")]
        public async Task<IActionResult> AddChapterTranslation(Guid webNovelChapterContentId)
        {
            var user = await _webNovelService.GetUserAsync(User);
            var model = await _webNovelService.GetWebNovelAddChapterTranslationViewModelAsync(user, webNovelChapterContentId);

            if (model.WebNovel.IsComplete)
                return View("IsCompleteError", model.WebNovel);
            
            return View("AddChapter", model);
        }
        
        [HttpGet]
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{webNovelChapterContentId}")]
        public async Task<IActionResult> EditChapterTranslation(Guid webNovelChapterContentId)
        {
            var user = await _webNovelService.GetUserAsync(User);
            var model = await _webNovelService.GetEditChapterTranslationViewModelAsync(user, webNovelChapterContentId);
            
            if (model == null)
                return RedirectToNotFound();
            
            return View(model);
        }
        
        [HttpPost]
        [Authorize(Roles = Constants.Permissions.WebNovelEdit)]
        [Route("[action]/{id}")]
        public async Task<IActionResult> EditChapterTranslation(EditChapterTranslationViewModel model)
        {
            var user = await _webNovelService.GetUserAsync(User);
            var spoofedModel = await _webNovelService.GetEditChapterTranslationViewModelAsync(user, model.WebNovelChapterContentId);
            model.Languages = spoofedModel.Languages;
            model.WebNovel = spoofedModel.WebNovel;
            model.WebNovelContent = spoofedModel.WebNovelContent;
            
            if (!ModelState.IsValid)
                return View(model);

            await _webNovelService.UpdateChapterContentAsync(user, model);
            return RedirectToAction(nameof(Read), new {webNovelUrl = model.WebNovel.UrlName, languageCode = model.WebNovelContent.Language.Code, volume = model.Volume, chapterNumber = model.Number});
        }

        [Authorize]
        [Route("notifications/subscribe/{webNovelUrl}")]
        public async Task<IActionResult> EnableNotifications(string webNovelUrl)
        {
            var user = await _webNovelService.GetUserAsync(User);
            await _webNovelService.UpdateNotificationStatus(user, webNovelUrl, true);
            return RedirectToAction(nameof(Details), new {webNovelUrl});
        }
        
        [Authorize]
        [Route("notifications/unsubscribe/{webNovelUrl}")]
        public async Task<IActionResult> DisableNotifications(string webNovelUrl)
        {
            var user = await _webNovelService.GetUserAsync(User);
            await _webNovelService.UpdateNotificationStatus(user, webNovelUrl, false);
            return RedirectToAction(nameof(Details), new {webNovelUrl});
        }
    }
}