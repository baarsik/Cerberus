using System.ComponentModel.DataAnnotations;
using Cerberus.Models.Extensions;
using Cerberus.Models.ValidationAttributes;

namespace Cerberus.Components.Models
{
    public class NewComment
    {
        [Required]
        [PureLengthLimit(10, 5000, "Minimum comment length is 10, maximum length is 5000")]
        public string Text { get; set; }
    }
}