using System;
using System.Collections.Generic;

namespace Cerberus.Models
{
    public class ForumHierarchyJson
    {
        public Guid Id { get; set; }
        public bool Enabled { get; set; }
        public ICollection<ForumHierarchyJson> Children { get; set; } = new List<ForumHierarchyJson>();
    }
}