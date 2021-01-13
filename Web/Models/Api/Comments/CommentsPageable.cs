using System;
using System.Collections.Generic;
using Web.Interfaces;
using Web.SafeModels;
using DataContext.Models;

namespace Web.Models.Api.Comments
{
    public class CommentsPageable : IPageableModel<CommentSafe>
    {
        public IList<CommentSafe> Items { get; set; } = new List<CommentSafe>();
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}