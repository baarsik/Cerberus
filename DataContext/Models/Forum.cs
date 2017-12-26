using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class Forum : BaseEntity
    {
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        public int DisplayOrderId { get; set; }

        // Role required to be this forum (+subforums) moderator
        [Required]
        [MaxLength(16)]
        public string ModeratorRole { get; set; }

        public virtual Forum Parent { get; set; }
    }
}
