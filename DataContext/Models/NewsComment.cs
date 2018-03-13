using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class NewsComment : BaseEntity
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public DateTime? LastEditDate { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public virtual ApplicationUser Author { get; set; }

        [Required]
        public virtual News News { get; set; }

        public override string ToString()
        {
            return Content;
        }
    }
}
