using System;
using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    public class CommentsController : Controller
    {
        private readonly CommentsService _commentsService;

        public CommentsController(CommentsService commentsService)
        {
            _commentsService = commentsService;
        }
        
        public async Task<IActionResult> GetComments(Guid entityId, int? page = 1)
        {
            return new JsonResult(await _commentsService.GetCommentsAsync(entityId, page ?? 1));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostComment(Guid entityId, string content)
        {
            var user = await _commentsService.GetUserAsync(User);
            var result = await _commentsService.AddCommentAsync(user, entityId, content);
            return new JsonResult(result);
        }
    }
}