using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DataContext.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(32)]
        public string DisplayName { get; set; }

        [Required]
        [MinLength(64)]
        [MaxLength(64)]
        public string ApiKey { get; set; }

        [MaxLength(15)]
        public string ApiBindedIp { get; set; }

        public Guid LastApiTokenId { get; set; }

        [MaxLength(100)]
        public string LockoutReason { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        [Required]
        public string Avatar { get; set; } = "noavatar";

        [MaxLength(8)]
        public string Culture { get; set; }
        
        [Required]
        public bool IsAdultContentConsentGiven { get; set; } = false;

        public virtual ICollection<ApplicationUserLanguage> UserLanguages { get; set; }

        public virtual ICollection<WebNovelReaderData> WebNovelReaderData { get; set; }
        
        public virtual ICollection<ApplicationUserNotifications> Notifications { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
