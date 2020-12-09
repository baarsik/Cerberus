using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Models.Extensions;
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
        public NotificationsService(ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
            : base(context, userManager, configuration)
        {
        }

        public async Task AddNewWebNovelChapterNotificationAsync(WebNovelChapterContent webNovelChapterContent)
        {
            var applicableUsers = await Db.WebNovelReaderData
                .Where(x => x.NotificationsEnabled &&
                            x.WebNovel.Id == webNovelChapterContent.Chapter.WebNovel.Id)
                .Select(x => x.User)
                .ToListAsync();

            var notifications = new List<ApplicationUserNotifications>();
            foreach (var user in applicableUsers)
            {
                var languages = user.GetUserOrDefaultLanguages(Db, Configuration);
                if (languages.All(language => language.Id != webNovelChapterContent.Language.Id))
                    continue;

                var webNovelName = await Db.WebNovelContent
                    .Where(x => x.WebNovel.Id == webNovelChapterContent.Chapter.WebNovel.Id &&
                                x.Language.Id == webNovelChapterContent.Language.Id)
                    .Select(x => x.Name)
                    .FirstOrDefaultAsync();

                var linkText = webNovelChapterContent.Chapter.WebNovel.UsesVolumes
                    ? $"<b>{webNovelName}</b> Vol. {webNovelChapterContent.Chapter.Volume} Chapter {webNovelChapterContent.Chapter.Number}"
                    : $"<b>{webNovelName}</b> Chapter {webNovelChapterContent.Chapter.Number}";

                var link = $"<a href=\"/wn/read/{webNovelChapterContent.Language.Code}/{webNovelChapterContent.Chapter.WebNovel.UrlName}/{webNovelChapterContent.Chapter.Number}/\">{linkText}</a>";
                notifications.Add(new ApplicationUserNotifications
                {
                    Body = $"New Release: {link}",
                    User = user
                });
            }

            Db.AddRange(notifications);
            await Db.SaveChangesAsync();

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

        public async Task DeleteNotificationAsync(ApplicationUser user, Guid notificationId)
        {
            var notification = await Db.ApplicationUserNotifications
                .FirstOrDefaultAsync(x => x.User.Id == user.Id && x.Id == notificationId);

            if (notification == null)
                return;

            notification.IsRead = true;
            notification.IsDeleted = true;
            Db.Update(notification);
            await Db.SaveChangesAsync();
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