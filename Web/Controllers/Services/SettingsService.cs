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
        public SettingsService(IDbContextFactory<ApplicationContext> dbContextFactory,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
            : base(dbContextFactory, userManager, configuration)
        {
        }
        
        public ForumSettingsViewModel GetForumSettingsViewModel()
        {
            using var context = DbContextFactory.CreateDbContext();
            var model = new ForumSettingsViewModel
            {
                ForumTree = context.Forums
                    .IgnoreQueryFilters()
                    .Include(c => c.Children)
                    .Include(c => c.Threads)
                    .Where(c => c.Parent == null)
                    .OrderBy(c => c.DisplayOrderId)
                    .Select(x => ToForumInfo(x, context))
                    .ToList()
            };
            return model;
        }

        public async Task CreateForum(CreateForumViewModel model)
        {
            await using var context = DbContextFactory.CreateDbContext();
            var forum = new Forum
            {
                Id = Guid.NewGuid(),
                DisplayOrderId = await context.Forums.MaxAsync(c => c.DisplayOrderId) + 1,
                IsEnabled = false,
                Title = model.Title.RemoveHTML()
            };
            context.Add(forum);
            
            await context.SaveChangesAsync();
        }
        
        public async Task UpdateForums(IEnumerable<ForumHierarchyJson> forumHierarchy)
        {
            await using var context = DbContextFactory.CreateDbContext();
            var displayOrderId = 0;
            foreach (var item in forumHierarchy)
            {
                displayOrderId = await UpdateForum(item, ++displayOrderId, null, context);
            }

            await context.SaveChangesAsync();
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
        
        private ForumInfo ToForumInfo(Forum forum, ApplicationContext context)
        {
            return new ForumInfo
            {
                Forum = forum,
                IsEnabled = forum.IsEnabled,
                ThreadCount = forum.Threads.Count,
                Children = context.Forums
                    .IgnoreQueryFilters()
                    .Include(c => c.Children)
                    .Include(c => c.Threads)
                    .Where(c => forum.Children.Select(x => x.Id).Contains(c.Id))
                    .OrderBy(c => c.DisplayOrderId)
                    .AsEnumerable()
                    .Select(x => ToForumInfo(x, context))
                    .ToList()
            };
        }
    }
}