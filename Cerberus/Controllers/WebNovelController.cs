using System;
using System.Threading.Tasks;
using Cerberus.Controllers.Services;
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

        [Route("{page?}")]
        public async Task<IActionResult> Index(int? page)
        {
            var model = await _webNovelService.GetWebNovelIndexViewModelAsync(page ?? 1);
            return View(model);
        }

        [Route("[action]/{webNovelUrl}")]
        public IActionResult Details(string webNovelUrl)
        {
            throw new NotImplementedException();
        }
        
        [Route("[action]/{webNovelUrl}/{chapterNumber}")]
        public IActionResult Read(string webNovelUrl, string chapterNumber)
        {
            throw new NotImplementedException();
        }
    }
}