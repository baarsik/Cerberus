using System;
using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
