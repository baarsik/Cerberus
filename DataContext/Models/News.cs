using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class News : BaseEntity
    {
        [Required]
        [MaxLength(64)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Uri { get; set; }

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime? LastEditDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<NewsComment> Comments { get; set; }
        
        public virtual ICollection<Attachment> Attachments { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
