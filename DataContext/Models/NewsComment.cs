using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class NewsComment : BaseEntity
    {
        [Required]
        public virtual string Content { get; set; }

        [Required]
        public virtual DateTime CreateDate { get; set; } = DateTime.Now;

        public virtual DateTime? LastEditDate { get; set; }

        [Required]
        public virtual bool IsDeleted { get; set; } = false;

        [Required]
        public virtual ApplicationUser Author { get; set; }

        [Required]
        public virtual News News { get; set; }
    }
}
