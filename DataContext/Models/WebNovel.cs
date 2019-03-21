using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class WebNovel : BaseEntity
    {
        [Required]
        public string UrlName { get; set; }

        public string CoverUrl { get; set; }
        
        public string OriginalName { get; set; }

        public string Author { get; set; }

        [Required]
        public bool UsesVolumes { get; set; } = true;
        
        [Required]
        public bool IsComplete { get; set; } = false;

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public virtual Country Country { get; set; }

        public virtual ICollection<WebNovelChapter> Chapters { get; set; }

        public virtual ICollection<WebNovelContent> Translations { get; set; }
    }
}