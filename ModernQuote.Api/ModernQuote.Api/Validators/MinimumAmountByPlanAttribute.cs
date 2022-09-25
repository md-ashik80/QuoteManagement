using ModernQuote.Api.Models;
using ModernQuote.Api.Rules;
using System;
using System.ComponentModel.DataAnnotations;

namespace ModernQuote.Api.Validators
{
    public class MinimumAmountByPlanAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var quoteRequest = validationContext.ObjectInstance as QuoteRequest;

            if (quoteRequest != null)
            {
                var pensionPlan = PensionPlanFactory.GetPlan(quoteRequest.PensionPlan);

                if (pensionPlan != null)
                {
                    return quoteRequest.InvestmentAmount >= pensionPlan.MinimumInvestmentAmount
                        ? ValidationResult.Success
                        : new ValidationResult(string.Format(Constants.Messages.InvestmentAmountLimitOfPlan, pensionPlan.MinimumInvestmentAmount, quoteRequest.PensionPlan));
                }
            }
            
            return new ValidationResult(Constants.Messages.InvestmentAmountOrPlanIsInvalid);
        }
    }
}
