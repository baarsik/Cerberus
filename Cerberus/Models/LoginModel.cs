using System;
using System.ComponentModel.DataAnnotations;

namespace Cerberus.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(64)]
        public string Login { get; set; }

        [Required]
        [MaxLength(64)]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
