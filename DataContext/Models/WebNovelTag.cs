using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class WebNovelTag : BaseEntity
    {
        [Required]
        public string FallbackName { get; set; }

        public virtual ICollection<WebNovelTagTranslation> Translations { get; set; }
        
        public virtual ICollection<WebNovelTagBinding> WebNovelBindings { get; set; }
    }
}