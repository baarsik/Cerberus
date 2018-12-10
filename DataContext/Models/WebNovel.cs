using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class WebNovel : BaseEntity
    {
        [Required]
        public string UrlName { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string CoverUrl { get; set; }
        
        public string OriginalName { get; set; }

        public string Author { get; set; }

        [Required]
        public bool UsesVolumes { get; set; } = true;

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        public virtual ICollection<WebNovelChapter> Chapters { get; set; }
    }
}