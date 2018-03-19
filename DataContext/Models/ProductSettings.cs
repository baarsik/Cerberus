using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class ProductSettings : BaseEntity
    {
        [Required]
        public virtual Product Product { get; set; }

        [Required]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [MaxLength(64)]
        public string Key { get; set; }

        [Required]
        [MaxLength(64)]
        public string Value { get; set; }
    }
}