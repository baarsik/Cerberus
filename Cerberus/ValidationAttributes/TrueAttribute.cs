using System;
using System.ComponentModel.DataAnnotations;

namespace Cerberus.ValidationAttributes
{
    public class TrueAttribute : ValidationAttribute
    {    
        public override bool IsValid(object obj)
        {
            if (obj is bool value)
                return value;

            throw new InvalidOperationException("Target object must be of a bool type");
        }
    }
}