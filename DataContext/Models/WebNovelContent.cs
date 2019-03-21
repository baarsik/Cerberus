using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class WebNovelContent : BaseEntity
    {   
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public virtual Language Language { get; set; }

        [Required]
        public virtual WebNovel WebNovel { get; set; }
    }
}