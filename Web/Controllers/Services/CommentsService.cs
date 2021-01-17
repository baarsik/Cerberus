using System;
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

namespace Web.Controllers.Services
{
    public class CommentsService : BaseService
    {
        public CommentsService(IDbContextFactory<ApplicationContext> dbContextFactory, UserManager<ApplicationUser> userManager, IConfiguration configuration)
            : base(dbContextFactory, userManager, configuration)
        {
        }

        public async Task<CommentsPageable> GetCommentsAsync(Guid relatedEntityId, int page)
        {
            await using var context = DbContextFactory.CreateDbContext();
            var totalComments = await context.Comments.CountAsync(x => x.RelatedEntityId == relatedEntityId);
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
            
            var comments = await context.Comments
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

            var context = DbContextFactory.CreateDbContext();
            var comment = new Comment
            {
                RelatedEntityId = entityId,
                Content = content.SanitizeStrictHTML(),
                Author = user
            };
            context.Add(comment);
            context.Entry(user).State = EntityState.Unchanged;
            await context.SaveChangesAsync();
            await context.DisposeAsync();

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

            await using var context = DbContextFactory.CreateDbContext();
            var comment = await context.Comments.FindAsync(id);
            comment.IsDeleted = true;
            context.Update(comment);
            await context.SaveChangesAsync();
        }
    }
}