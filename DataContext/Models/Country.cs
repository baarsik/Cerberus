using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class Country : BaseEntity
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string GlobalName { get; set; }

        [Required]
        public string LocalName { get; set; }
    }
}