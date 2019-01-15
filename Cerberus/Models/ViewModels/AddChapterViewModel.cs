using System;
using System.ComponentModel.DataAnnotations;
using DataContext.Models;

namespace Cerberus.Models.ViewModels
{
    public class AddChapterViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int Volume { get; set; }
        
        [Required]
        public int Number { get; set; }
        
        [MinLength(3)]
        [MaxLength(64)]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public WebNovel WebNovel { get; set; }
    }
}