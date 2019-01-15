using System;
using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Microsoft.AspNetCore.Authorization;
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
        
        [Authorize(Roles = Constants.Roles.Admin + "," + Constants.Roles.WebNovelEditor)]
        [Route("[action]/{webNovelId}")]
        public async Task<IActionResult> AddChapter(Guid webNovelId)
        {
            var model = await _webNovelService.GetWebNovelAddChapterViewModelAsync(webNovelId);
            return View(model);
        }
    }
}