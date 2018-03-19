using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class ProductSharedData : BaseEntity
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
        
        [Required]
        public virtual Product Product { get; set; }
    }
}