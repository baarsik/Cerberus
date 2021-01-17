using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Extensions;
using Web.Models.ViewModels.Notifications;
using DataContext;
using DataContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Web.Controllers.Services
{
    public class NotificationsService : BaseService
    {
        public NotificationsService(IDbContextFactory<ApplicationContext> dbContextFactory,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
            : base(dbContextFactory, userManager, configuration)
        {
        }

        public async Task AddNewWebNovelChapterNotificationAsync(WebNovelChapterContent webNovelChapterContent, ApplicationContext context)
        {
            var applicableUsers = await context.WebNovelReaderData
                .Where(x => x.NotificationsEnabled &&
                            x.WebNovel.Id == webNovelChapterContent.Chapter.WebNovel.Id)
                .Select(x => x.User)
                .ToListAsync();

            var notifications = new List<ApplicationUserNotifications>();
            foreach (var user in applicableUsers)
            {
                var languages = user.GetUserOrDefaultLanguages(context, Configuration);
                if (languages.All(language => language.Id != webNovelChapterContent.Language.Id))
                    continue;

                var webNovelName = await context.WebNovelContent
                    .Where(x => x.WebNovel.Id == webNovelChapterContent.Chapter.WebNovel.Id &&
                                x.Language.Id == webNovelChapterContent.Language.Id)
                    .Select(x => x.Name)
                    .FirstOrDefaultAsync();

                var linkText = webNovelChapterContent.Chapter.WebNovel.UsesVolumes
                    ? $"<b>{webNovelName}</b> Vol. {webNovelChapterContent.Chapter.Volume} Chapter {webNovelChapterContent.Chapter.Number}"
                    : $"<b>{webNovelName}</b> Chapter {webNovelChapterContent.Chapter.Number}";

                if (!string.IsNullOrEmpty(webNovelChapterContent.Title))
                {
                    linkText = $"{linkText} – {webNovelChapterContent.Title}";
                }

                var link = $"<a href=\"/read/{webNovelChapterContent.Language.Code}/{webNovelChapterContent.Chapter.WebNovel.UrlName}/{webNovelChapterContent.Chapter.Volume}/{webNovelChapterContent.Chapter.Number}/\">{linkText}</a>";
                notifications.Add(new ApplicationUserNotifications
                {
                    Body = $"New Release: {link}",
                    User = user
                });
            }

            context.AddRange(notifications);
            await context.SaveChangesAsync();

        }
        
        public async Task<NotificationsIndexViewModel> GetNotificationsIndexViewModelAsync(ApplicationUser user, int page)
        {
            await using var context = DbContextFactory.CreateDbContext();
            var itemsToDisplayCount = await context.ApplicationUserNotifications.CountAsync(c => c.User == user);
            var totalPages = (int) Math.Ceiling(itemsToDisplayCount / (double) Constants.Notifications.ItemsPerIndexPage);
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

            model.Items = await context.ApplicationUserNotifications
                .Where(c => c.User == user)
                .OrderByDescending(c => c.NotificationDate)
                .Skip(Constants.Notifications.ItemsPerIndexPage * (model.Page - 1))
                .Take(Constants.Notifications.ItemsPerIndexPage)
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
                await MarkAsRead(user, page, context);
            }

            return model;
        }

        public async Task<ManageNotificationsViewModel> GetManageNotificationsViewModelAsync(ApplicationUser user, int page)
        {
            await using var context = DbContextFactory.CreateDbContext();
            var languages = user.GetUserOrDefaultLanguages(context, Configuration);
            
            var itemsToDisplayCount = await context.WebNovelReaderData
                .CountAsync(c =>
                    c.User.Id == user.Id &&
                    c.NotificationsEnabled);
            var totalPages = (int) Math.Ceiling(itemsToDisplayCount / (double) Constants.Notifications.ItemsPerIndexPage);
            if (totalPages == 0)
            {
                totalPages = 1;
            }
            
            var model = new ManageNotificationsViewModel
            {
                Page = page < 1
                    ? 1
                    : page > totalPages
                        ? totalPages
                        : page,
                TotalPages = totalPages,
            };

            model.Items = await context.WebNovelReaderData
                .Include(x => x.WebNovel)
                .ThenInclude(x => x.Translations)
                    .ThenInclude(x => x.Language)
                .Where(readerData => readerData.User.Id == user.Id && readerData.NotificationsEnabled)
                .Select(x => new
                {
                    ReaderData = x,
                    WebNovelContent = x.WebNovel.GetTranslation(languages)
                })
                .Select(x => new ManageNotificationsViewModelItem
                {
                    Id = x.ReaderData.Id,
                    WebNovelContent = x.WebNovelContent,
                    WebNovelURL = $"/details/{x.ReaderData.WebNovel.UrlName}/"
                })
                .Skip(Constants.Notifications.ItemsPerIndexPage * (model.Page - 1))
                .Take(Constants.Notifications.ItemsPerIndexPage)
                .ToListAsync();
            
            return model;
        }

        public async Task DeleteNotificationAsync(ApplicationUser user, Guid notificationId)
        {
            await using var context = DbContextFactory.CreateDbContext();
            var notification = await context.ApplicationUserNotifications
                .FirstOrDefaultAsync(x => x.User.Id == user.Id && x.Id == notificationId);

            if (notification == null)
                return;

            notification.IsRead = true;
            notification.IsDeleted = true;
            context.Update(notification);
            await context.SaveChangesAsync();
        }

        private async Task MarkAsRead(ApplicationUser user, int page, ApplicationContext context)
        {
            var notifications = context.ApplicationUserNotifications
                .Where(c => c.User == user)
                .OrderByDescending(c => c.NotificationDate)
                .Take(Constants.Notifications.ItemsPerIndexPage)
                .Skip(Constants.Notifications.ItemsPerIndexPage * (page - 1));

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            context.UpdateRange(notifications);
            await context.SaveChangesAsync();
        }
    }
}