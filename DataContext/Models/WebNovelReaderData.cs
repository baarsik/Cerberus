using System.ComponentModel.DataAnnotations;
using DataContext.EnumDataTypes;

namespace DataContext.Models
{
    public class WebNovelReaderData : BaseEntity
    {
        [Required]
        public bool NotificationsEnabled { get; set; } = false;

        public int? Rating { get; set; }

        public ReadingStatus ReadingStatus { get; set; } = ReadingStatus.None;

        public SortOrder SortOrder { get; set; } = SortOrder.FromNewToOld;
        
        [Required]
        public virtual WebNovel WebNovel { get; set; }

        public virtual WebNovelChapter LastOpenedChapter { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}