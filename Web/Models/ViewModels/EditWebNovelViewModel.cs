using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewModels
{
    public class EditWebNovelViewModel
    {
        [Required]
        [ReadOnly(true)]
        public Guid TranslationId { get; set; }

        public string UrlName { get; set; }
        
        public string OriginalName { get; set; }

        public string LanguageName { get; set; }
     
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }

        public string Author { get; set; }

        [Required]
        public bool IsAdultContent { get; set; }
        
        [Required]
        public bool UsesVolumes { get; set; }
    }
}