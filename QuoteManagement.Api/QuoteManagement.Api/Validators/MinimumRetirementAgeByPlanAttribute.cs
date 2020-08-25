using QuoteManagement.Api.Models;
using QuoteManagement.Api.Rules;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteManagement.Api.Validators
{
    public class MinimumRetirementAgeByPlanAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var quoteRequest = validationContext.ObjectInstance as QuoteRequest;

            if (quoteRequest != null)
            {
                var pensionPlan = PensionPlanFactory.GetPlan(quoteRequest.PensionPlan);

                if (pensionPlan != null)
                {
                    return quoteRequest.RetirementAge >= pensionPlan.MinimumRetirementAge
                        ? ValidationResult.Success
                        : new ValidationResult(string.Format(Constants.Messages.RetirementAgeLimitOfPlan, pensionPlan.MinimumRetirementAge, quoteRequest.PensionPlan));
                }
            }

            return new ValidationResult(Constants.Messages.RetirementAgeOrPlanIsInvalid);
        }
    }
}
