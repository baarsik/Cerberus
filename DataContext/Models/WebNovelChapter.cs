using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class WebNovelChapter : BaseEntity
    {
        [Required]
        public int Volume { get; set; }
        
        [Required]
        public int Number { get; set; }
        
        public string Title { get; set; }
        
        [Required]
        public string Text { get; set; }
        
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        [Required]
        public DateTime FreeToAccessDate { get; set; } = DateTime.Now;
        
        [Required]
        public virtual ApplicationUser Uploader { get; set; }
        
        [Required]
        public virtual WebNovel WebNovel { get; set; }
        
        public virtual WebNovelChapter PreviousChapter { get; set; }
        public virtual WebNovelChapter NextChapter { get; set; }
    }
}