using System;

namespace QuoteManagement.Api.Rules
{
    public interface IMaturityAmountCalculator
    {
        decimal Execute(string plan, DateTime dateOfBirth, DateTime quoteDate, int retirementAge, decimal investmentAmount);
    }
}
