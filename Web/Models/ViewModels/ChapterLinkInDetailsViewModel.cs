using System.Collections.Generic;
using System.Linq;
using DataContext.Models;

namespace Web.Models.ViewModels
{
    public class ChapterLinkInDetailsViewModel
    {
        public WebNovel WebNovel { get; set; }
        public IList<WebNovelChapter> Chapters { get; set; }
        public IList<Language> DisplayedLanguages { get; set; }
        public bool HasEditorAccess { get; set; }
    }
}