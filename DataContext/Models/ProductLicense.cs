using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class ProductLicense : BaseEntity
    {
        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsBlocked { get; set; } = false;
        
        [Required]
        public virtual Product Product { get; set; }
        
        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}