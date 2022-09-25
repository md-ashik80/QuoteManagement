using System;

using NUnit.Framework;
using ModernQuote.Api.Rules;
using ModernQuote.Api.Constants;
using ModernQuote.Api.Test;

namespace QuoteManagement.Api.Test.Rules
{
    public class MaturityAmountCalculatorTests
    {
        [Test]
        [TestCase(PensionPlan.Silver.Name)]
        [TestCase(PensionPlan.Gold.Name)]
        [TestCase(PensionPlan.Platinum.Name)]
        public void ExecuteForGivenPlanWithValidInvestmentAmountAndRetirmentAgeReturnsCalculatedMaturityAmount(string testPlan)
        {
            var pensionPlan = PensionPlanFactory.GetPlan(testPlan);
            var quoteDate = DateTime.Today;
            var dateOfBirth = quoteDate.AddYears(-40);
            var investmentAmount = pensionPlan.MinimumInvestmentAmount;
            var retirementAge = pensionPlan.MinimumRetirementAge;

            var maturityAmount = new MaturityAmountCalculator().Execute(testPlan, dateOfBirth, quoteDate, retirementAge, investmentAmount);

            var currentAge = quoteDate.Date.Year - dateOfBirth.Date.Year;
            var duration = retirementAge - currentAge;

            var expectedMaturityAmount = investmentAmount * (1 + pensionPlan.Factor) * duration / 100.0m;

            Assert.AreEqual(expectedMaturityAmount, maturityAmount, "Expected MaturityAmount is not calculated");
            
        }

        [Test]
        public void ExecuteGivenWithUnknownPlanIsNotCalculated()
        {
            var quote = new QuoteRequestBuilder()
                .WithSilverPlanQuote()
                .WithEligibleSilverPlanInvestmentAmount()
                .WithUnknownPlan().Build();

            Assert.AreEqual(0.0m, new MaturityAmountCalculator().Execute(quote.PensionPlan, quote.DateOfBirth.Value,
                                                                         quote.QuoteDate, quote.RetirementAge.Value,
                                                                         quote.InvestmentAmount.Value), "Expected MaturityAmount is not returned");
        }
    }
}
