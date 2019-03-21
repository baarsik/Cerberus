using System;
using System.Collections.Generic;
using DataContext.Models;

namespace Cerberus.Models
{
    public class WebNovelInfo
    {
        public WebNovel WebNovel { get; set; }
        public WebNovelContent WebNovelContent { get; set; }
        public WebNovelChapterContent LastChapterTranslation { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int TotalChapters { get; set; }
        public int TotalVolumes { get; set; }
        public ICollection<Language> UserLanguages { get; set; }
    }
}