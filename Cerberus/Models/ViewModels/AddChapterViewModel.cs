using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class AddChapterViewModel
    {
        [Required]
        public Guid WebNovelId { get; set; }

        [Required]
        public int Volume { get; set; }
        
        [Required]
        public int Number { get; set; }
        
        [MinLength(3)]
        [MaxLength(64)]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:" + Constants.Misc.DateFormat + "}")]
        public DateTime FreeToAccessDate { get; set; } = DateTime.Today;

        [Required]
        public Guid LanguageId { get; set; }

        public IEnumerable<Language> Languages { get; set; }
        
        public WebNovel WebNovel { get; set; }
    }
}