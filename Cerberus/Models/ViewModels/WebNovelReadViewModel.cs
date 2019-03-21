using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class WebNovelReadViewModel
    {
        public WebNovelContent WebNovelContent { get; set; }
        public WebNovelChapterContent Translation { get; set; }
        public WebNovelChapterContent NextChapterContent { get; set; }
        public WebNovelChapterContent PrevChapterContent { get; set; }
    }
}