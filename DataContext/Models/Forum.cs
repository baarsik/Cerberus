using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class Forum : BaseEntity
    {
        [Required]
        [MaxLength(64)]
        public string Title { get; set; }

        [Required]
        public int DisplayOrderId { get; set; }

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
