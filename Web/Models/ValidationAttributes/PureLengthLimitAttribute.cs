using System.ComponentModel.DataAnnotations;
using Web.Models.Extensions;

namespace Web.Models.ValidationAttributes
{
    public class PureLengthLimitAttribute : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;
        private readonly string _errorMessage;

        public PureLengthLimitAttribute(int minLength, int maxLength, string errorMessage)
        {
            _minLength = minLength;
            _maxLength = maxLength;
            _errorMessage = errorMessage;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not string strValue)
            {
                return new ValidationResult("Not a string");
            }

            var length = strValue.GetPureTextLength();
            if (length < _minLength || length > _maxLength)
            {
                return new ValidationResult(_errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}