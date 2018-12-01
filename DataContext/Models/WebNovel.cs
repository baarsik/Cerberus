using System;
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
        
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        public virtual WebNovelChapter FirstChapter { get; set; }
    }
}