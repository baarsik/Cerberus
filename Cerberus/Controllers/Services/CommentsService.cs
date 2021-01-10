using System;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Models.Api.Comments;
using Cerberus.Models.Extensions;
using Cerberus.SafeModels;
using DataContext;
using DataContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cerberus.Controllers.Services
{
    public class CommentsService : BaseService
    {
        public CommentsService(ApplicationContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
            : base(context, userManager, configuration)
        {
        }

        public async Task<CommentsPageable> GetCommentsAsync(Guid relatedEntityId, int page)
        {
            var totalComments = await Db.Comments.CountAsync(x => x.RelatedEntityId == relatedEntityId);
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
            
            var comments = await Db.Comments
                .Where(x => x.RelatedEntityId == relatedEntityId)
                .Include(x => x.Author)
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
                Content = content.RemoveHTML(),
                Author = user
            };
            Db.Add(comment);
            await Db.SaveChangesAsync();

            return comment;
        }
    }
}