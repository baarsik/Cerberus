using System.Collections.Generic;
using DataContext.Models;

namespace Cerberus.Models
{
    public class ForumSettingsViewModel
    {
        public IList<ForumInfo> ForumTree { get; set; }
    }

    public class ForumInfo
    {
        public Forum Forum { get; set; }
        public int ThreadCount { get; set; }
        public IList<ForumInfo> Children { get; set; }
    }
}