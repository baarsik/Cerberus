using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext.Models
{
    public class WebNovelChapterContent : BaseEntity
    {
        public string Title { get; set; }
        
        [Required]
        public string Text { get; set; }

        public string SourceURL { get; set; }
        
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        [Required]
        public DateTime FreeToAccessDate { get; set; } = DateTime.Now;

        public int Symbols { get; set; }

        [Required]
        public Guid LanguageId { get; set; }
        
        [ForeignKey(nameof(LanguageId))]
        public virtual Language Language { get; set; }
        
        [Required]
        [MaxLength(450)]
        public string UploaderId { get; set; }
        
        [ForeignKey(nameof(UploaderId))]
        public virtual ApplicationUser Uploader { get; set; }
        
        [Required]
        public Guid ChapterId { get; set; }
        
        [ForeignKey(nameof(ChapterId))]
        public WebNovelChapter Chapter { get; set; }
        
        public virtual ICollection<WebNovelChapterContentReaderData> ReaderData { get; set; }
    }
}