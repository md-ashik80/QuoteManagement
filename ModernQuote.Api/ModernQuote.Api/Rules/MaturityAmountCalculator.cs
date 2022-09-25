using System;

namespace ModernQuote.Api.Rules
{
    public class MaturityAmountCalculator : IMaturityAmountCalculator
    {
        public decimal Execute(string plan, DateTime dateOfBirth, DateTime quoteDate, int retirementAge, decimal investmentAmount)
        {
            var pensionPlan = PensionPlanFactory.GetPlan(plan);

            if(pensionPlan != null)
            {
                var currentAge = quoteDate.Date.Year - dateOfBirth.Date.Year;
                var duration = retirementAge - currentAge;

                return investmentAmount * (1 + pensionPlan.Factor) * duration / 100.0m;
            }

            return 0.0m;
        }
    }
}
