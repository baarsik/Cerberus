using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class News : BaseEntity
    {
        [Required]
        [MaxLength(64)]
        public virtual string Title { get; set; }

        [Required]
        public virtual string Content { get; set; }

        [Required]
        public string Uri { get; set; }

        [Required]
        public virtual DateTime CreateDate { get; set; } = DateTime.Now;

        public virtual DateTime? LastEditDate { get; set; }

        [Required]
        public virtual bool IsDeleted { get; set; } = false;

        [Required]
        public virtual ApplicationUser Author { get; set; }
    }
}
