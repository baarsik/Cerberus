using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class PrivateMessage : BaseEntity
    {
        [Required]
        [MaxLength(64)]
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;
        
        [Required]
        public virtual ApplicationUser Receiver { get; set; }

        [Required]
        public bool IsSentByAdministration { get; set; } = false;
        
        public virtual ApplicationUser Sender { get; set; }
    }
}