using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class ApplicationUserNotifications : BaseEntity
    {
        [Required]
        public string Body { get; set; }
        
        [Required]
        public DateTime NotificationDate { get; set; } = DateTime.Now;

        [Required]
        public bool IsRead { get; set; } = false;

        [Required]
        public bool IsDeleted { get; set; } = false;
        
        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}