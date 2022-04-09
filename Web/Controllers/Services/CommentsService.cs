using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Text;
using Web.Models.Api.Comments;
using Web.Models.Extensions;
using Web.SafeModels;
using DataContext;
using DataContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Controllers.Services
{
    public class CommentsService : BaseService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CommentsService(ApplicationContext dbContext, UserManager<ApplicationUser> userManager, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
            : base(dbContext, userManager, configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<int> GetCommentCountAsync(Guid relatedEntityId)
        {
            return await Db.Comments.AsNoTracking().CountAsync(x => x.RelatedEntityId == relatedEntityId);
        }
        
        public async Task<IDictionary<Guid, int>> GetCommentCountAsync(IEnumerable<Guid> relatedEntityIds)
        {
            return (await Db.Comments
                .AsNoTracking()    
                .Where(x => relatedEntityIds.Contains(x.RelatedEntityId))
                .ToListAsync())
                .GroupBy(x => x.RelatedEntityId)
                .ToDictionary(x => x.Key, x => x.Count());
        }
        
        public async Task<CommentsPageable> GetCommentsAsync(Guid relatedEntityId, int page)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var ownDbContext = scope.ServiceProvider.GetService<ApplicationContext>() ?? Db; 
            
            var totalComments = await ownDbContext.Comments.AsNoTracking().CountAsync(x => x.RelatedEntityId == relatedEntityId);
            var totalPages = (int) Math.Ceiling(totalComments / (double) Constants.Comments.ItemsPerPage);
            if (totalPages == 0)
            {
                totalPages = 1;
            }
            
            var currentPage = page < 1
                ? 1
                : page > totalPages
                    ? totalPages
                    : page;
            
            var comments = await ownDbContext.Comments
                .AsNoTracking()
                .Where(x => x.RelatedEntityId == relatedEntityId)
                .Include(x => x.Author)
                .OrderByDescending(x => x.CreateDate)
                .Skip(Constants.Comments.ItemsPerPage * (currentPage - 1))
                .Take(Constants.Comments.ItemsPerPage)
                .ToListAsync();

            return new CommentsPageable
            {
                Items = comments.Select(CommentSafe.Convert).ToList(),
                Page = currentPage,
                TotalPages = totalPages
            };
        }

        public async Task<Comment> AddCommentAsync(ApplicationUser user, Guid entityId, string content)
        {
            if (!user.HasWriteAccess())
                return null;

            var comment = new Comment
            {
                RelatedEntityId = entityId,
                Content = content.SanitizeStrictHTML(),
                Author = user
            };
            Db.Add(comment);
            Db.Entry(user).State = EntityState.Unchanged;
            await Db.SaveChangesAsync();

            return comment;
        }

        public async Task DeleteCommentAsync(ApplicationUser user, Guid id)
        {
            var hasModerateAccess = false;
            foreach (var role in Constants.Permissions.Moderate.SplitCommas())
            {
                hasModerateAccess |= await UserManager.IsInRoleAsync(user, role);
            }

            if (!hasModerateAccess)
            {
                return;
            }

            var comment = await Db.Comments.FindAsync(id);
            comment.IsDeleted = true;
            Db.Update(comment);
            await Db.SaveChangesAsync();
        }
    }
}