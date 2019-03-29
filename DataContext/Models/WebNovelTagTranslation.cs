using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class WebNovelTagTranslation : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Language Language { get; set; }

        [Required]
        public WebNovelTag Tag { get; set; }
    }
}