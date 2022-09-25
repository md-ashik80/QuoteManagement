using System.ComponentModel.DataAnnotations;

using NUnit.Framework;
using ModernQuote.Api.Rules;
using ModernQuote.Api.Validators;

namespace ModernQuote.Api.Test.Validators
{
    public class MinimumAmountByPlanAttributeTests
    {
        [Test]
        [TestCase(Constants.PensionPlan.Silver.Name)]
        [TestCase(Constants.PensionPlan.Gold.Name)]
        [TestCase(Constants.PensionPlan.Platinum.Name)]
        public void GetValidationResultByPlanWithEligibleInvestmentGivenThenValidationIsPassing(string testPlan)
        {
            var pensionPlan = PensionPlanFactory.GetPlan(testPlan);

            var quoteRequest = new QuoteRequestBuilder().WithPlan(testPlan)
                .WithInvestmentAmount(pensionPlan.MinimumInvestmentAmount + 1000).Build();

            var validator = new MinimumAmountByPlanAttribute();

            var result = validator.GetValidationResult(quoteRequest.InvestmentAmount, new ValidationContext(quoteRequest));

            Assert.IsNull(result);

        }

        [Test]
        [TestCase(Constants.PensionPlan.Silver.Name)]
        [TestCase(Constants.PensionPlan.Gold.Name)]
        [TestCase(Constants.PensionPlan.Platinum.Name)]
        public void GetValidationResultByPlanWithIneligibleInvestmentGivenThenValidationIsFailing(string testPlan)
        {
            var pensionPlan = PensionPlanFactory.GetPlan(testPlan);

            var quoteRequest = new QuoteRequestBuilder().WithPlan(testPlan)
                .WithInvestmentAmount(pensionPlan.MinimumInvestmentAmount - 1000).Build();

            var validator = new MinimumAmountByPlanAttribute();

            var result = validator.GetValidationResult(quoteRequest.InvestmentAmount, new ValidationContext(quoteRequest));

            var expectedMessage = string.Format(Constants.Messages.InvestmentAmountLimitOfPlan, pensionPlan.MinimumInvestmentAmount, testPlan);

            Assert.AreEqual(expectedMessage, result.ErrorMessage);

        }

        [Test]
        public void GetValidationResultUnknowPlanGivenThenValidationIsFailing()
        {
            var quoteRequest = new QuoteRequestBuilder().WithUnknownPlan().Build();

            var validator = new MinimumAmountByPlanAttribute();

            var result = validator.GetValidationResult(quoteRequest.InvestmentAmount, new ValidationContext(quoteRequest));

            Assert.AreEqual(Constants.Messages.InvestmentAmountOrPlanIsInvalid, result.ErrorMessage);

        }
    }
}
