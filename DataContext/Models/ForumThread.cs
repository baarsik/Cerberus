using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class ForumThread : BaseEntity
    {
        [Required]
        [MaxLength(64)]
        public string Title { get; set; }
        
        [Required]
        public virtual ForumThreadReply MainReply { get; set; }
        
        [Required]
        public virtual ForumThreadReply LastReply { get; set; }
        
        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Required]
        public bool IsPinned { get; set; } = false;

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public bool IsClosed { get; set; } = false;

        [Required]
        public virtual Forum Forum { get; set; }

        [Required]
        public virtual ApplicationUser Author { get; set; }
        
        public virtual ICollection<ForumThreadReply> Replies { get; set; }
        
        public virtual ICollection<Attachment> Attachments { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
