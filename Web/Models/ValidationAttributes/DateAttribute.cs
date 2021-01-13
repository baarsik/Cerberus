using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Web.Models.ValidationAttributes
{
    public class DateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime)
            {
            }
            else
            {
                var parsed = DateTime.TryParseExact((string) value, Constants.Misc.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
                if (!parsed)
                    return new ValidationResult("Could not parse a date");
            }
            
            return ValidationResult.Success;
        }
    }
}