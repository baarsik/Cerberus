using System.Collections.Generic;
using DataContext.Models;

namespace Cerberus.Models
{
    public class WebNovelIndexViewModel
    {
        public IEnumerable<WebNovel> WebNovels { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}