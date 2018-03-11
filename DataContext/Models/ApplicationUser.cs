using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DataContext.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(32)]
        public string DisplayName { get; set; }

        [MaxLength(24)]
        public virtual string FirstName { get; set; }

        [MaxLength(32)]
        public virtual string LastName { get; set; }

        [MaxLength(32)]
        public virtual string MiddleName { get; set; }

        [MaxLength(100)]
        public virtual string LockoutReason { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        [Required]
        public string Avatar { get; set; } = "noavatar";

        [MaxLength(8)]
        public virtual string Culture { get; set; }
    }
}
