using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cerberus.Models.ValidationAttributes;
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
        [FutureOrCurrentDate]
        public string FreeToAccessDate { get; set; } = DateTime.Today.ToString(Constants.Misc.DateFormat);

        [Required]
        public Guid LanguageId { get; set; }

        public IEnumerable<Language> Languages { get; set; }
        
        public WebNovel WebNovel { get; set; }
    }
}