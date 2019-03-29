using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class WebNovelTagBinding : BaseEntity
    {
        [Required]
        public WebNovel WebNovel { get; set; }

        [Required]
        public WebNovelTag Tag { get; set; }
    }
}