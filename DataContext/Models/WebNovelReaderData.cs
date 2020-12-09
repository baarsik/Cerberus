using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class WebNovelReaderData : BaseEntity
    {
        [Required]
        public bool NotificationsEnabled { get; set; } = false;

        public int? Rating { get; set; }
        
        [Required]
        public virtual WebNovel WebNovel { get; set; }

        public virtual WebNovelChapter LastOpenedChapter { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}