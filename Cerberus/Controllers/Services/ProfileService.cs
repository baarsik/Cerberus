using System.Threading.Tasks;
using Cerberus.Models;
using DataContext;
using DataContext.Models;
using Microsoft.EntityFrameworkCore;

namespace Cerberus.Controllers.Services
{
    public sealed class ProfileService
    {
        private readonly ApplicationContext _db;

        public ProfileService(ApplicationContext context)
        {
            _db = context;
        }
        
        public async Task<ProfileStatistics> GetUserStatisticsAsync(ApplicationUser user)
        {
            var statistics = new ProfileStatistics
            {
                TotalNews = await _db.News.CountAsync(c => c.Author == user),
                TotalNewsComments = await _db.NewsComments.CountAsync(c => c.Author == user),
                TotalForumThreads = await _db.ForumThreads.CountAsync(c => c.Author == user),
                TotalForumPosts = await _db.ForumThreadReplies.CountAsync(c => c.Author == user)
            };
            return statistics;
        }
    }
}