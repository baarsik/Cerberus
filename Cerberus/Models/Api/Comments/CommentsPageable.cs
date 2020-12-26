using System.Collections.Generic;
using Cerberus.Interfaces;
using DataContext.Models;

namespace Cerberus.Models.Api.Comments
{
    public class CommentsPageable : IPageableModel<Comment>
    {
        public IEnumerable<Comment> Items { get; set; } = new List<Comment>();
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}