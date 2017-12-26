using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class ForumThread : BaseEntity
    {
        [Required]
        public virtual DateTime CreateDate { get; set; } = DateTime.Now;

        [Required]
        public bool IsPinned { get; set; } = false;

        [Required]
        public virtual bool IsDeleted { get; set; } = false;

        [Required]
        public virtual bool IsClosed { get; set; } = false;

        [Required]
        public virtual Forum Forum { get; set; }

        [Required]
        public virtual User Author { get; set; }
    }
}
