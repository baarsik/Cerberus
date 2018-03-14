using DataContext.Models;

namespace Cerberus.Models
{
    public class ProfileViewModel
    {
        public bool IsOwnProfile { get; set; }
        
        public ApplicationUser User { get; set; }
    }
}