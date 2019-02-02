using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class AddWebNovelViewModel
    {
        [Required]
        public string Name { get; set; }
        
        public string OriginalName { get; set; }
        
        [Required]
        public string UrlName { get; set; }
        
        [Required]
        public string Description { get; set; }

        public string Author { get; set; }
        
        [Required]
        public bool UsesVolumes { get; set; }
    }
}