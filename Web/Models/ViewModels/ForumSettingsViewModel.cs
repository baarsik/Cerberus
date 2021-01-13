using System.Collections.Generic;
using DataContext.Models;

namespace Web.Models.ViewModels
{
    public class ForumSettingsViewModel
    {
        public IEnumerable<ForumInfo> ForumTree { get; set; }
    }

    public class ForumInfo
    {
        public Forum Forum { get; set; }
        public bool IsEnabled { get; set; }
        public int ThreadCount { get; set; }
        public IEnumerable<ForumInfo> Children { get; set; }
    }
}