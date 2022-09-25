using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ModernQuote.Api.Validators
{
    public class PossibleValuesAttribute : ValidationAttribute
    {
        public string[] ExpectedValues { get; set; }

        public string FieldName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ExpectedValues.Contains(value.ToString()) ? ValidationResult.Success :
                new ValidationResult(string.Format(@"{0} should be one from ({1})", FieldName, String.Join(",", ExpectedValues)));
        }
    }
}
