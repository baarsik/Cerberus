using System.ComponentModel.DataAnnotations;

namespace Cerberus.Components.Models
{
    public class NewComment
    {
        [Required]
        public string Text { get; set; }
    }
}