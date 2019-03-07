﻿using System.Collections.Generic;
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
        public ProfileNewsStatistics News { get; set; }
        public ProfileForumStatistics Forum { get; set; }
        public ProfileWebNovelStatistics WebNovel { get; set; }
    }

    public class ProfileNewsStatistics
    {
        public int TotalNews { get; set; }
        public int TotalComments { get; set; }
    }

    public class ProfileForumStatistics
    {
        public int TotalStartedThreads { get; set; }
        public int TotalReplies { get; set; }
    }

    public class ProfileWebNovelStatistics
    {
        public int TotalWebNovels { get; set; }
        public int TotalChapters { get; set; }
    }
}