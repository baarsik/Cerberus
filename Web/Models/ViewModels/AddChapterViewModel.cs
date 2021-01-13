﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Web.Models.ValidationAttributes;
using DataContext.Models;

namespace Web.Models.ViewModels
{
    public class AddChapterViewModel
    {
        [Required]
        public Guid WebNovelId { get; set; }

        [Required]
        public bool IsTranslation { get; set; }

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
        public bool IsAdultContent { get; set; }
        
        [Required]
        [FutureOrCurrentDate]
        public string FreeToAccessDate { get; set; } = DateTime.Today.ToString(Constants.Misc.DateFormat);

        [Required]
        public Guid LanguageId { get; set; }

        public IEnumerable<Language> Languages { get; set; }
        
        public WebNovel WebNovel { get; set; }

        public WebNovelContent WebNovelContent { get; set; }
    }
}