using DataContext.EnumDataTypes;
using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class WebNovelDetailsViewModel
    {
        public WebNovelInfo WebNovelInfo { get; set; }
        public ReaderUserData ReaderData { get; set; }
        public bool IsValid { get; set; }

        public class ReaderUserData
        {
            public bool NotificationsEnabled { get; set; } = false;
            public ChapterLinkInfo LastOpenedChapter { get; set; }
            public SortOrder SortOrder { get; set; } = SortOrder.FromNewToOld;
        }

        public class ChapterLinkInfo
        {
            public int? Volume { get; set; }
            public int Number { get; set; }
            public string LanguageCode { get; set; }
        }
    }
}