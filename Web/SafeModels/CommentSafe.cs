using System;
using DataContext.Models;

namespace Web.SafeModels
{
    public class CommentSafe
    {
        public Guid Id { get; set; }
        
        public Guid RelatedEntityId { get; set; }
        
        public DateTime CreateDate { get; set; }

        public DateTime? LastEditDate { get; set; }
        
        public string Content { get; set; }

        public ApplicationUserSafe Author { get; set; }
        
        public static CommentSafe Convert(Comment comment) =>
            new()
            {
                Id = comment.Id,
                RelatedEntityId = comment.RelatedEntityId,
                CreateDate = comment.CreateDate,
                LastEditDate = comment.LastEditDate,
                Content = comment.Content,
                Author = ApplicationUserSafe.Convert(comment.Author)
            };
    }
}