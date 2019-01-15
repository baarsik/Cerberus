using System.ComponentModel.DataAnnotations;
using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(64)]
        public string OldPassword { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(64)]
        public string NewPassword { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password does not match")]
        [MaxLength(64)]
        public string ConfirmNewPassword { get; set; }
        
        public ApplicationUser User { get; set; }
    }
}