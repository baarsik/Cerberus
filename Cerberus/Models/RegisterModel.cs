using System.ComponentModel.DataAnnotations;

namespace Cerberus.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(64)]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(64)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [MaxLength(64)]
        public string ConfirmPassword { get; set; }
    }
}
