using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext.Models
{
    public class Forum : BaseEntity
    {
        [Required]
        [MaxLength(64)]
        public string Title { get; set; }

        [Required]
        public int DisplayOrderId { get; set; }

        public Guid? ParentId { get; set; }
        
        [ForeignKey("ParentId")]
        public virtual Forum Parent { get; set; }
        
        public virtual ICollection<Forum> Children { get; set; }
        
        public virtual ICollection<ForumThread> Threads { get; set; }
        
        public virtual ICollection<ForumModerator> Moderators { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
