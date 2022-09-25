using System.ComponentModel.DataAnnotations;

using NUnit.Framework;
using ModernQuote.Api.Rules;
using ModernQuote.Api.Validators;

namespace ModernQuote.Api.Test.Validators
{
    class MinimumRetirementAgeByPlanAttributeTests
    {
        [Test]
        [TestCase(Constants.PensionPlan.Silver.Name)]
        [TestCase(Constants.PensionPlan.Gold.Name)]
        [TestCase(Constants.PensionPlan.Platinum.Name)]
        public void GetValidationResultByPlanWithEligibleRetirementAgeGivenThenValidationIsPassing(string testPlan)
        {
            var pensionPlan = PensionPlanFactory.GetPlan(testPlan);

            var quoteRequest = new QuoteRequestBuilder().WithPlan(testPlan)
                .WithRetirementAge(pensionPlan.MinimumRetirementAge + 2)
                .Build();

            var validator = new MinimumRetirementAgeByPlanAttribute();

            var result = validator.GetValidationResult(quoteRequest.RetirementAge, new ValidationContext(quoteRequest));

            Assert.IsNull(result);

        }

        [Test]
        [TestCase(Constants.PensionPlan.Silver.Name)]
        [TestCase(Constants.PensionPlan.Gold.Name)]
        [TestCase(Constants.PensionPlan.Platinum.Name)]
        public void GetValidationResultByPlanWithIneligibleRetirementAgeGivenThenValidationIsFailing(string testPlan)
        {
            var pensionPlan = PensionPlanFactory.GetPlan(testPlan);

            var quoteRequest = new QuoteRequestBuilder().WithPlan(testPlan)
                .WithRetirementAge(pensionPlan.MinimumRetirementAge - 1)
                .Build();

            var validator = new MinimumRetirementAgeByPlanAttribute();

            var result = validator.GetValidationResult(quoteRequest.RetirementAge, new ValidationContext(quoteRequest));

            var expectedMessage = string.Format(Constants.Messages.RetirementAgeLimitOfPlan, pensionPlan.MinimumRetirementAge, testPlan);
            Assert.AreEqual(expectedMessage, result.ErrorMessage);

        }

        [Test]
        public void GetValidationResultUnknowPlanGivenThenValidationIsFailing()
        {
            var quoteRequest = new QuoteRequestBuilder().WithUnknownPlan().Build();

            var validator = new MinimumRetirementAgeByPlanAttribute();

            var result = validator.GetValidationResult(quoteRequest.RetirementAge, new ValidationContext(quoteRequest));

            Assert.AreEqual(Constants.Messages.RetirementAgeOrPlanIsInvalid, result.ErrorMessage);

        }
    }
}
