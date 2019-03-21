using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class AddWebNovelViewModel
    {
        [Required]
        public string UrlName { get; set; }
        
        public string OriginalName { get; set; }
        
        [Required]
        public Guid LanguageId { get; set; }
     
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }

        public string Author { get; set; }
        
        [Required]
        public bool UsesVolumes { get; set; }
        
        public IEnumerable<Language> Languages { get; set; }
    }
}