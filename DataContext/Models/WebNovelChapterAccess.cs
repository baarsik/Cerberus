using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class WebNovelChapterAccess : BaseEntity
    {
        [Required]
        public ApplicationUser User { get; set; }
        
        [Required]
        public WebNovelChapter Chapter { get; set; }

        [Required]
        public DateTime AccessPurchaseDateTime { get; set; } = DateTime.Now;
    }
}