using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Models;
using Cerberus.Models.ViewModels;
using DataContext;
using DataContext.Models;
using Microsoft.EntityFrameworkCore;

namespace Cerberus.Controllers.Services
{
    public sealed class SettingsService
    {
        private readonly ApplicationContext _db;

        public SettingsService(ApplicationContext context)
        {
            _db = context;
        }
        
        public ForumSettingsViewModel GetForumSettingsViewModel()
        {
            var model = new ForumSettingsViewModel
            {
                ForumTree = _db.Forums
                    .IgnoreQueryFilters()
                    .Include(c => c.Children)
                    .Include(c => c.Threads)
                    .Where(c => c.Parent == null)
                    .OrderBy(c => c.DisplayOrderId)
                    .Select(ToForumInfo)
                    .ToList()
            };
            return model;
        }

        public async Task CreateForum(CreateForumViewModel model)
        {
            var forum = new Forum
            {
                Id = Guid.NewGuid(),
                DisplayOrderId = await _db.Forums.MaxAsync(c => c.DisplayOrderId) + 1,
                IsEnabled = false,
                Title = model.Title
            };
            _db.Add(forum);
            
            await _db.SaveChangesAsync();
        }
        
        public async Task UpdateForums(IEnumerable<ForumHierarchyJson> forumHierarchy)
        {
            var displayOrderId = 0;
            foreach (var item in forumHierarchy)
            {
                displayOrderId = await UpdateForum(item, ++displayOrderId, null);
            }

            await _db.SaveChangesAsync();
        }

        private async Task<int> UpdateForum(ForumHierarchyJson item, int displayOrderId, Guid? parentId)
        {
            var forum = await _db.Forums.IgnoreQueryFilters().SingleOrDefaultAsync(c => c.Id == item.Id);
            
            if (forum == null)
            {
                return displayOrderId;
            }
            
            forum.IsEnabled = item.Enabled;
            forum.ParentId = parentId;
            forum.DisplayOrderId = displayOrderId;
            _db.Update(forum);

            foreach (var child in item.Children)
            {
                await UpdateForum(child, ++displayOrderId, item.Id);
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
                Children = _db.Forums
                    .IgnoreQueryFilters()
                    .Include(c => c.Children)
                    .Include(c => c.Threads)
                    .Where(c => forum.Children.Select(x => x.Id).Contains(c.Id))
                    .OrderBy(c => c.DisplayOrderId)
                    .AsEnumerable()
                    .Select(ToForumInfo)
                    .ToList()
            };
        }
    }
}