using System;

using NUnit.Framework;
using QuoteManagement.Api.Rules;

namespace QuoteManagement.Api.Test.Rules
{
    public class MaturityAmountCalculatorTests
    {
        [Test]
        [TestCase(Constants.PensionPlan.Silver.Name)]
        [TestCase(Constants.PensionPlan.Gold.Name)]
        [TestCase(Constants.PensionPlan.Platinum.Name)]
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
