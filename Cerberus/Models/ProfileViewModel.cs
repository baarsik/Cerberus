using DataContext.Models;

namespace Cerberus.Models
{
    public class ProfileViewModel
    {
        public bool IsOwnProfile { get; set; }
        
        public ApplicationUser User { get; set; }
        
        public ProfileStatistics Statistics { get; set; }
    }

    public class ProfileStatistics
    {
        public int TotalNews { get; set; } = 0;

        public int TotalNewsComments { get; set; } = 0;

        public int TotalForumThreads { get; set; } = 0;
        
        public int TotalForumPosts { get; set; } = 0;
    }
}