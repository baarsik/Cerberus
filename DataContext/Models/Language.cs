using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class Language : BaseEntity
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string GlobalName { get; set; }
        
        [Required]
        public string LocalName { get; set; }

        [Required]
        public string CountryFlagIconName { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Language);
        }

        protected bool Equals(Language other)
        {
            return other != null && Code == other.Code;
        }

        public override int GetHashCode()
        {
            return Code != null ? Code.GetHashCode() : 0;
        }
    }
}