using System.Collections.Generic;
using System.Linq;
using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class ChapterLinkInDetailsViewModel
    {
        public WebNovel WebNovel { get; set; }
        public IOrderedEnumerable<WebNovelChapter> Chapters { get; set; }
        public ICollection<Language> DisplayedLanguages { get; set; }
    }
}