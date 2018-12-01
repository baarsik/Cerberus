using System.ComponentModel.DataAnnotations;

namespace DataContext.Models
{
    public class Settings : BaseEntity
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public SettingsType Type { get; set; } = SettingsType.String;
        
        [Required]
        public string Value { get; set; }
    }

    public enum SettingsType
    {
        String = 1,
        Bool = 2,
        Int = 3,
        Double = 4,
        DateTime = 5
    }
}