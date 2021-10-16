using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models;
using Web.Models.Extensions;
using Web.Models.ViewModels;
using DataContext;
using DataContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Web.Controllers.Services
{
    public sealed class SettingsService : BaseService
    {
        public SettingsService(ApplicationContext dbContext,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
            : base(dbContext, userManager, configuration)
        {
        }
        
        public ForumSettingsViewModel GetForumSettingsViewModel()
        {
            var model = new ForumSettingsViewModel
            {
                ForumTree = Db.Forums
                    .IgnoreQueryFilters()
                    .Include(c => c.Children)
                    .Include(c => c.Threads)
                    .Where(c => c.Parent == null)
                    .OrderBy(c => c.DisplayOrderId)
                    .ToList()
                    .Select(ToForumInfo)
                    .ToList()
            };
            return model;
        }

        public async Task CreateForum(CreateForumViewModel model)
        {
            var forumsAreNotEmpty = await Db.Forums.AnyAsync();
            var forum = new Forum
            {
                Id = Guid.NewGuid(),
                DisplayOrderId =  forumsAreNotEmpty ? await Db.Forums.MaxAsync(c => c.DisplayOrderId) + 1 : 1,
                IsEnabled = false,
                Title = model.Title.RemoveHTML()
            };
            Db.Add(forum);
            
            await Db.SaveChangesAsync();
        }
        
        public async Task UpdateForums(IEnumerable<ForumHierarchyJson> forumHierarchy)
        {
            var displayOrderId = 0;
            foreach (var item in forumHierarchy)
            {
                displayOrderId = await UpdateForum(item, ++displayOrderId, null, Db);
            }

            await Db.SaveChangesAsync();
        }

        private async Task<int> UpdateForum(ForumHierarchyJson item, int displayOrderId, Guid? parentId, ApplicationContext context)
        {
            var forum = await context.Forums.IgnoreQueryFilters().SingleOrDefaultAsync(c => c.Id == item.Id);
            
            if (forum == null)
            {
                return displayOrderId;
            }
            
            forum.IsEnabled = item.Enabled;
            forum.ParentId = parentId;
            forum.DisplayOrderId = displayOrderId;
            context.Update(forum);

            foreach (var child in item.Children)
            {
                await UpdateForum(child, ++displayOrderId, item.Id, context);
            }

            return displayOrderId;
        }
        
        private ForumInfo ToForumInfo(Forum forum)
        {
            return new ForumInfo
            {
                Forum = forum,
                IsEnabled = forum.IsEnabled,
                ThreadCount = forum.Threads.Count,
                Children = Db.Forums
                    .IgnoreQueryFilters()
                    .Include(c => c.Children)
                    .Include(c => c.Threads)
                    .Where(c => forum.Children.Select(x => x.Id).Contains(c.Id))
                    .OrderBy(c => c.DisplayOrderId)
                    .ToList()
                    .Select(ToForumInfo)
                    .ToList()
            };
        }
    }
}