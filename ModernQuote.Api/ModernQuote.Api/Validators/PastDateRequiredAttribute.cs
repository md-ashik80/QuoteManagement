using System;
using System.ComponentModel.DataAnnotations;

namespace ModernQuote.Api.Validators
{
    public class PastDateRequiredAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateValue = Convert.ToDateTime(value);

            return dateValue.Date < DateTime.Today;
        }
    }
}
