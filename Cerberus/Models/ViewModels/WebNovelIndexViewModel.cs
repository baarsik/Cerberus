using System.Collections.Generic;
using Cerberus.Interfaces;
using DataContext.Models;

namespace Cerberus.Models
{
    public class WebNovelIndexViewModel : IPageableViewModel<WebNovelInfo>
    {
        public IEnumerable<WebNovelInfo> Items { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}