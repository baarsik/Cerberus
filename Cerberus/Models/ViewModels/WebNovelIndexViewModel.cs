using System.Collections.Generic;
using Cerberus.Interfaces;

namespace Cerberus.Models.ViewModels
{
    public class WebNovelIndexViewModel : IPageableViewModel<WebNovelInfo>
    {
        public IEnumerable<WebNovelInfo> Items { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }
}