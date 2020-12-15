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
    public sealed class ProfileService : BaseService
    {
        public ProfileService(ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        : base(context, userManager, configuration)
        {}
        
        public async Task<ProfileStatistics> GetUserStatisticsAsync(ApplicationUser user)
        {
            var statistics = new ProfileStatistics
            {
                Forum = new ProfileForumStatistics
                {
                    TotalStartedThreads = await Db.ForumThreads.CountAsync(c => c.Author == user),
                    TotalReplies = await Db.ForumThreadReplies.CountAsync(c => c.Author == user)
                },
                WebNovel = new ProfileWebNovelStatistics
                {
                    TotalWebNovels = await Db.WebNovels
                        .Include(c => c.Chapters)
                            .ThenInclude(c => c.Translations)
                            .ThenInclude(c => c.Uploader)
                        .CountAsync(c => c.Chapters.Any(x => x.Translations.Any(z => z.Uploader == user))),
                    TotalChapters = await Db.WebNovelChapters
                        .Include(c => c.Translations)
                        .CountAsync(c => c.Translations.Any(x => x.Uploader == user))
                }
            };
            return statistics;
        }
    }
}