using System.ComponentModel.DataAnnotations;

namespace Cerberus.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(64)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(64)]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
