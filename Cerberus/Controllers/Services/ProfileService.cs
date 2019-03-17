using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Models.ViewModels;
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
                News = new ProfileNewsStatistics
                {
                    TotalNews = await _db.News.CountAsync(c => c.Author == user),
                    TotalComments = await _db.NewsComments.CountAsync(c => c.Author == user)
                },
                Forum = new ProfileForumStatistics
                {
                    TotalStartedThreads = await _db.ForumThreads.CountAsync(c => c.Author == user),
                    TotalReplies = await _db.ForumThreadReplies.CountAsync(c => c.Author == user)
                },
                WebNovel = new ProfileWebNovelStatistics
                {
                    TotalWebNovels = await _db.WebNovels
                        .Include(c => c.Chapters)
                            .ThenInclude(c => c.Translations)
                            .ThenInclude(c => c.Uploader)
                        .CountAsync(c => c.Chapters.Any(x => x.Translations.Any(z => z.Uploader == user))),
                    TotalChapters = await _db.WebNovelChapters
                        .Include(c => c.Translations)
                        .CountAsync(c => c.Translations.Any(x => x.Uploader == user))
                }
            };
            return statistics;
        }
        
        public async Task<ICollection<ProductLicense>> GetProductLicensesAsync(ApplicationUser user)
        {
            return await _db.ProductLicenses
                .Include(c => c.User)
                .Include(c => c.Product)
                .Where(c => c.User == user)
                .OrderByDescending(c => c.EndDate)
                .ToListAsync();
        }
    }
}