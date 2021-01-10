using System.Collections.Generic;
using Cerberus.Interfaces;

namespace Cerberus.Models.ViewModels
{
    public class WebNovelIndexViewModel : IPageableModel<WebNovelInfo>
    {
        public IList<WebNovelInfo> Items { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}