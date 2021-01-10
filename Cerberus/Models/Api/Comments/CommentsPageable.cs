using System;
using System.Collections.Generic;
using Cerberus.Interfaces;
using Cerberus.SafeModels;
using DataContext.Models;

namespace Cerberus.Models.Api.Comments
{
    public class CommentsPageable : IPageableModel<CommentSafe>
    {
        public IList<CommentSafe> Items { get; set; } = new List<CommentSafe>();
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}