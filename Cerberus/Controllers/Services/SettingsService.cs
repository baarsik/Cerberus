﻿using System.Linq;
using Cerberus.Models;
using DataContext;
using DataContext.Models;

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
                ThreadCount = forum.Threads.Count,
                Children = forum.Children.Select(ToForumInfo).ToList()
            };
        }
    }
}