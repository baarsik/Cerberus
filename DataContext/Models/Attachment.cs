using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class Attachment : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Required]
        public string Uri { get; set; }

        [Required]
        public virtual ApplicationUser Uploader { get; set; }

        public virtual ForumThread ForumThread { get; set; }

        public virtual News News { get; set; }

        public virtual Product Product { get; set; }

        public virtual ICollection<AttachmentDownloads> Downloads { get; set; }
    }
}
