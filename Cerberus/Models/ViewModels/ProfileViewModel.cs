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
        public int TotalNews { get; set; }

        public int TotalNewsComments { get; set; }

        public int TotalForumThreads { get; set; }
        
        public int TotalForumPosts { get; set; }
    }
}