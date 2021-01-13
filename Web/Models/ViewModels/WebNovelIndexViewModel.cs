using System.Collections.Generic;
using Web.Interfaces;

namespace Web.Models.ViewModels
{
    public class WebNovelIndexViewModel : IPageableModel<WebNovelInfo>
    {
        public IList<WebNovelInfo> Items { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}