using System.ComponentModel.DataAnnotations;

namespace Cerberus.Models
{
    public class CreateForumViewModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(64)]
        public string Title { get; set; }
    }
}