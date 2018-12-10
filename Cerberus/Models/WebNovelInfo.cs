using DataContext.Models;

namespace Cerberus.Models
{
    public class WebNovelInfo
    {
        public WebNovel WebNovel { get; set; }
        public WebNovelChapter LastChapter { get; set; }
        public int TotalChapters { get; set; }
        public int TotalVolumes { get; set; }
    }
}