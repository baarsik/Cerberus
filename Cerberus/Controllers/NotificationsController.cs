using System;
using System.Threading.Tasks;
using Cerberus.Controllers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Controllers
{
    [Authorize]
    public class NotificationsController : BaseController
    {
        private readonly NotificationsService _notificationsService;

        public NotificationsController(NotificationsService notificationsService)
        {
            _notificationsService = notificationsService;
        }

        [Route("[controller]/{page:int=1}")]
        public async Task<IActionResult> Index(int? page)
        {
            var user = await _notificationsService.GetUserAsync(User);
            var model = await _notificationsService.GetNotificationsIndexViewModelAsync(user, page ?? 1);
            return View(model);
        }
        
        public async Task<IActionResult> Delete(Guid notificationId, int? page)
        {
            var user = await _notificationsService.GetUserAsync(User);
            await _notificationsService.DeleteNotificationAsync(user, notificationId);
            return RedirectToAction(nameof(Index), new {page});
        }

        public async Task<IActionResult> ManageNotifications(int? page)
        {
            var user = await _notificationsService.GetUserAsync(User);
            var model = await _notificationsService.GetManageNotificationsViewModelAsync(user, page ?? 1);
            return View(model);
        }
    }
}