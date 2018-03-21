using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
        
        [Required]
        public bool RequiresLicense { get; set; } = true;
        
        public string Description { get; set; }
        
        public virtual ICollection<Attachment> Attachments { get; set; }
        
        public virtual ICollection<ProductLicense> Licenses { get; set; }
        
        public virtual ICollection<ProductSharedData> SharedData { get; set; }
    }
}