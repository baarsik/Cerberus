using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class WebNovelReadViewModel
    {
        public WebNovelChapterContent Translation { get; set; }
        public WebNovelChapterContent NextChapterContent { get; set; }
        public WebNovelChapterContent PrevChapterContent { get; set; }
    }
}