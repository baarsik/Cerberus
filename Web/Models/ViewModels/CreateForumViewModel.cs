using System.ComponentModel.DataAnnotations;

namespace Web.Models.ViewModels
{
    public class CreateForumViewModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(64)]
        public string Title { get; set; }
    }
}