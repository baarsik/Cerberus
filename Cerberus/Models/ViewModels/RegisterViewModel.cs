using System.ComponentModel.DataAnnotations;
using Cerberus.Models.ValidationAttributes;

namespace Cerberus.Models.ViewModels
{
    public class RegisterViewModel
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
        [Compare(nameof(Password), ErrorMessage = "Password does not match")]
        [MaxLength(64)]
        public string ConfirmPassword { get; set; }
        
        [Required(ErrorMessage = "Please read and accept rules")]
        [True(ErrorMessage = "Please read and accept rules")]
        public bool AreRulesAccepted { get; set; }
    }
}