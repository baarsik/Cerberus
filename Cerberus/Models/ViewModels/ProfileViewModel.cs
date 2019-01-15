using System.Collections.Generic;
using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class ProfileViewModel
    {
        public bool IsOwnProfile { get; set; }
        
        public ApplicationUser User { get; set; }
        
        public ProfileStatistics Statistics { get; set; }
        
        public ICollection<ProductLicense> ProductLicenses { get; set; }
    }

    public class ProfileStatistics
    {
        public int TotalNews { get; set; } = 0;

        public int TotalNewsComments { get; set; } = 0;

        public int TotalForumThreads { get; set; } = 0;
        
        public int TotalForumPosts { get; set; } = 0;
    }
}