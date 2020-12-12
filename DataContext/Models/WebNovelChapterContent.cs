using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class WebNovelChapterContent : BaseEntity
    {
        public string Title { get; set; }
        
        [Required]
        public string Text { get; set; }
        
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        [Required]
        public DateTime FreeToAccessDate { get; set; } = DateTime.Now;

        public int Symbols { get; set; }

        [Required]
        public virtual Language Language { get; set; }
        
        [Required]
        public virtual ApplicationUser Uploader { get; set; }
        
        [Required]
        public WebNovelChapter Chapter { get; set; }
    }
}