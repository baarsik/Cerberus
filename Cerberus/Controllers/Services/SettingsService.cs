using System.Linq;
using Cerberus.Models;
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