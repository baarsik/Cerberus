using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Web.Models.ValidationAttributes
{
    public class FutureOrCurrentDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime dateTime;
            if (value is DateTime date)
            {
                dateTime = date;
            }
            else
            {
                var parsed = DateTime.TryParseExact((string) value, Constants.Misc.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                if (!parsed)
                    return new ValidationResult("Could not parse a date");
            }

            if (dateTime < DateTime.Now.Date)
                return new ValidationResult("Date must be the same or future date");
            
            return ValidationResult.Success;
        }
    }
}