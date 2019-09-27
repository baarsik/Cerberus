using System;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Models.ViewModels;
using DataContext;
using DataContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cerberus.Controllers.Services
{
    public class NotificationsService : BaseService
    {
        public NotificationsService(ApplicationContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
            : base(context, userManager, configuration)
        {
        }

        public async Task<NotificationsIndexViewModel> GetNotificationsIndexViewModelAsync(ApplicationUser user, int page)
        {
            var itemsToDisplayCount = await Db.ApplicationUserNotifications.CountAsync(c => c.User == user);
            var totalPages = (int) Math.Ceiling(itemsToDisplayCount / (double) Constants.Notifications.NotificationsPerIndexPage);
            if (totalPages == 0)
            {
                totalPages = 1;
            }
            
            var model = new NotificationsIndexViewModel
            {
                Page = page < 1
                    ? 1
                    : page > totalPages
                        ? totalPages
                        : page,
                TotalPages = totalPages,
            };

            model.Items = await Db.ApplicationUserNotifications
                .Where(c => c.User == user)
                .OrderByDescending(c => c.NotificationDate)
                .Take(Constants.WebNovel.ItemsPerIndexPage)
                .Skip(Constants.WebNovel.ItemsPerIndexPage * (model.Page - 1))
                .Select(c => new NotificationsIndexViewModelItem
                {
                    Id = c.Id,
                    Body = c.Body,
                    IsRead = c.IsRead,
                    NotificationDate = c.NotificationDate
                })
                .ToListAsync();

            if (model.Items.Any(c => !c.IsRead))
            {
                await MarkAsRead(user, page);
            }

            return model;
        }

        private async Task MarkAsRead(ApplicationUser user, int page)
        {
            var notifications = Db.ApplicationUserNotifications
                .Where(c => c.User == user)
                .OrderByDescending(c => c.NotificationDate)
                .Take(Constants.WebNovel.ItemsPerIndexPage)
                .Skip(Constants.WebNovel.ItemsPerIndexPage * (page - 1));

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            Db.UpdateRange(notifications);
            await Db.SaveChangesAsync();
        }
    }
}