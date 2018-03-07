using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class AttachmentDownloads : BaseEntity
    {
        [Required]
        public virtual Attachment Attachment { get; set; }

        [Required]
        public virtual User User { get; set; }
    }
}
