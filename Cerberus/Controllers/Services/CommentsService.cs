using System;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Interfaces;
using Cerberus.Models.Api;
using Cerberus.Models.Api.Comments;
using Cerberus.Models.Extensions;
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

        public async Task<IPageableModel<Comment>> GetCommentsAsync(Guid relatedEntityId, int itemsPerPage, int page)
        {
            var totalComments = await Db.Comments.CountAsync(x => x.RelatedEntityId == relatedEntityId);
            var totalPages = (int) Math.Ceiling(totalComments / (double) itemsPerPage);
            if (totalPages == 0)
            {
                totalPages = 1;
            }

            var model = new CommentsPageable
            {
                Page = page < 1
                    ? 1
                    : page > totalPages
                        ? totalPages
                        : page,
                TotalPages = totalPages
            };
            
            model.Items = await Db.Comments
                .Where(x => x.RelatedEntityId == relatedEntityId)
                .Include(x => x.Author)
                .Skip(itemsPerPage * (model.Page - 1))
                .Take(itemsPerPage)
                .ToListAsync();

            return model;
        }

        public async Task<Comment> AddCommentAsync(ApplicationUser user, Guid entityId, string content)
        {
            if (!user.HasWriteAccess())
                return null;

            var comment = new Comment
            {
                RelatedEntityId = entityId,
                Content = content.RemoveHTML()
            };
            Db.Add(comment);
            await Db.SaveChangesAsync();

            return comment;
        }
    }
}