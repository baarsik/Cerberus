using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class WebNovelChapter : BaseEntity
    {
        [Required]
        public int Volume { get; set; }
        
        [Required]
        public int Number { get; set; }
        
        [Required]
        public bool IsAdultContent { get; set; } = false;
        
        [Required]
        public virtual WebNovel WebNovel { get; set; }
        
        public virtual WebNovelChapter PreviousChapter { get; set; }
        
        public virtual WebNovelChapter NextChapter { get; set; }
        
        public ICollection<WebNovelChapterContent> Translations { get; set; }
    }
}