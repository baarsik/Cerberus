using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class ForumThreadReply : BaseEntity
    {
        [Required]
        public string Content { get; set; }
        
        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime? LastEditDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public bool IsMainReply { get; set; } = false;

        [Required]
        public virtual ForumThread Thread { get; set; }

        [Required]
        public virtual ApplicationUser Author { get; set; }

        public override string ToString()
        {
            return Content;
        }
    }
}
