using System.ComponentModel.DataAnnotations;

namespace Cerberus.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(64)]
        public string Login { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(64)]
        public string Password { get; set; }
    }
}
