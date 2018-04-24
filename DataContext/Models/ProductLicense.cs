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

        public bool IsExpired() => EndDate <= DateTime.Now;
        
        public override string ToString()
        {
            var timeLeft = EndDate - DateTime.Now;
            var remainingTime = IsBlocked
                ? "License is blocked"
                : EndDate <= DateTime.Now
                    ? "Expired"
                    : timeLeft.TotalHours <= 24
                        ? $"{(int)timeLeft.TotalHours} hour{((int)timeLeft.TotalHours == 1 ? "" : "s")} {timeLeft.Minutes} minute{(timeLeft.Minutes == 1 ? "" : "s")} remaining"
                        : $"{(int)timeLeft.TotalDays} days remaining";

            return $"{Product?.Name ?? "<Unknown>"} ({remainingTime})";
        }
    }
}