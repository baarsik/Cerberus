using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class ApplicationUserCoinUpdate : BaseEntity
    {
        [Required]
        public int Change { get; set; }
        
        [Required]
        public int NewValue { get; set; }
        
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        
        [Required]
        public ApplicationUser User { get; set; }
    }
}