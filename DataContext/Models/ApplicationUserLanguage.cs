using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class ApplicationUserLanguage : BaseEntity
    {
        [Required]
        public int Priority { get; set; }

        [Required]
        public Language Language { get; set; }
        
        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}