using System;
using System.ComponentModel.DataAnnotations;

namespace Cerberus.Models
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
        [MustBeTrue(ErrorMessage = "Please read and accept rules")]
        public bool AreRulesAccepted { get; set; }
    }
    
    public class MustBeTrue : ValidationAttribute
    {    
        public override bool IsValid(object obj)
        {
            if (obj is bool value)
                return value;

            throw new InvalidOperationException("This attribute is only valid for Foo objects");
        }
    }
}