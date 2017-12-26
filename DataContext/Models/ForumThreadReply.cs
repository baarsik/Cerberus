using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class ForumThreadReply : BaseEntity
    {
        [Required]
        public virtual DateTime CreateDate { get; set; } = DateTime.Now;

        public virtual DateTime? LastEditDate { get; set; }

        [Required]
        public virtual bool IsDeleted { get; set; } = false;

        [Required]
        public virtual Forum Forum { get; set; }

        [Required]
        public virtual ForumThread Thread { get; set; }

        [Required]
        public virtual User Author { get; set; }
    }
}
