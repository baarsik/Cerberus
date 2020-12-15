using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class Comment : BaseEntity
    {
        [Required]
        public Guid RelatedEntityId { get; set; }
        
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime? LastEditDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;
        
        [Required]
        public string Content { get; set; }
        
        [Required]
        public virtual ApplicationUser Author { get; set; }
    }
}