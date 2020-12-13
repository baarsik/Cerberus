using System.ComponentModel.DataAnnotations;
using DataContext.EnumDataTypes;

namespace DataContext.Models
{
    public class WebNovelChapterContentReaderData : BaseEntity
    {
        [Required]
        public bool IsRead { get; set; } = true;

        public int? Rating { get; set; }
        
        [Required]
        public virtual WebNovelChapterContent ChapterContent { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}