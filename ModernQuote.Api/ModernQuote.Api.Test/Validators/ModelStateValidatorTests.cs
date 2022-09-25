using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using NUnit.Framework;
using ModernQuote.Api.Constants;
using ModernQuote.Api.Models;
using ModernQuote.Api.Rules;

namespace ModernQuote.Api.Test.Validators
{
    public class ModelStateValidatorTests
    {
        [Test]
        public void RequiredFieldsAreValidated()
        {
            var result = new List<ValidationResult>();
            var quoteRequest = new QuoteRequest();
            
            var isValid = Validator.TryValidateObject(quoteRequest, new ValidationContext(quoteRequest), result);

            Assert.IsFalse(isValid);

            var expectedMessages = new string[]
            {
                "The Name field is required.",
                "The Sex field is required.",
                "The DateOfBirth field is required.",
                "The PensionPlan field is required.",
                "The InvestmentAmount field is required.",
                "The RetirementAge field is required.",
                "The Email field is required.",
                "The MobileNumber field is required."
            };

            Assert.AreEqual(expectedMessages.Length, result.Count);

            Assert.IsTrue(result.All(r => expectedMessages.Contains(r.ErrorMessage)));


        }

        [Test]
        public void NameExceedsMaxLengthValidationFailed()
        {
            var quoteRequest = new QuoteRequestBuilder().
                WithSilverPlanQuote().
                WithExceedLengthName().Build();

            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(quoteRequest, new ValidationContext(quoteRequest), result, true);

            Assert.IsFalse(isValid);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Name should not exceeds 30 characters", result[0].ErrorMessage, "Name characters exceeds error is not occurred");
        }

        [Test]
        public void InvalidSexValidationFailed()
        {
            var quoteRequest = new QuoteRequestBuilder().
                WithSilverPlanQuote().
                WithInvalidSex().Build();

            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(quoteRequest, new ValidationContext(quoteRequest), result, true);

            Assert.IsFalse(isValid);

            Assert.AreEqual(1, result.Count);

            var expectedMessage = "Sex should be one from (Male,Female)";

            Assert.AreEqual(expectedMessage, result[0].ErrorMessage, "Invalid sex error is not occurred");
        }

        [Test]
        public void InvalidDateOfBirthValidationFailed()
        {
            var quoteRequest = new QuoteRequestBuilder().
                WithSilverPlanQuote().
                WithInvalidDateOfBirth().Build();

            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(quoteRequest, new ValidationContext(quoteRequest), result, true);

            Assert.IsFalse(isValid);

            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(Constants.Messages.PastDateOfBirthRequired, result[0].ErrorMessage, "Invalid DateOfBirth is not occurred");
        }

        [Test]
        public void InvalidEmailValidationFailed()
        {
            var quoteRequest = new QuoteRequestBuilder().
                WithSilverPlanQuote().
                WithInvalidEmail().Build();

            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(quoteRequest, new ValidationContext(quoteRequest), result, true);

            Assert.IsFalse(isValid);

            Assert.AreEqual(1, result.Count);

            Assert.AreEqual("The Email field is not a valid e-mail address.", result[0].ErrorMessage, "Invalid Email is not occurred");
        }

        [Test]
        public void InvalidMobileNumberValidationFailed()
        {
            var quoteRequest = new QuoteRequestBuilder().
                WithSilverPlanQuote().
                WithInvalidMobileNumber().Build();

            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(quoteRequest, new ValidationContext(quoteRequest), result, true);

            Assert.IsFalse(isValid);

            Assert.AreEqual(1, result.Count);

            Assert.AreEqual("MobileNumber should be 10 digits.", result[0].ErrorMessage, "Invalid MobileNumber is not occurred");
        }

        
        [Test]
        public void UnknownPlanValidationFailed()
        {
            var quoteRequest = new QuoteRequestBuilder().
                WithSilverPlanQuote().
                WithUnknownPlan().Build();

            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(quoteRequest, new ValidationContext(quoteRequest), result, true);

            Assert.IsFalse(isValid);

            Assert.AreEqual(3, result.Count);

            var expectedMessage = "PensionPlan should be one from (PensionSilver,PensionGold,PensionPlatinum)";

            Assert.AreEqual(expectedMessage, result[0].ErrorMessage, "Invalid Plan error is not occurred");
        }

        [Test]
        [TestCase(Constants.PensionPlan.Silver.Name)]
        [TestCase(Constants.PensionPlan.Gold.Name)]
        [TestCase(Constants.PensionPlan.Platinum.Name)]
        public void PlanWithIneligibleInvestmentAmountValidationFailed(string testPlan)
        {
            var plan = PensionPlanFactory.GetPlan(testPlan);

            var quoteRequest = new QuoteRequestBuilder().
                WithPlanQuote(testPlan).
                WithInvestmentAmount(plan.MinimumInvestmentAmount - 1000).Build();

            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(quoteRequest, new ValidationContext(quoteRequest), result, true);

            Assert.IsFalse(isValid);

            var expectedMessage = string.Format(Constants.Messages.InvestmentAmountLimitOfPlan, plan.MinimumInvestmentAmount, testPlan);

            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(expectedMessage, result[0].ErrorMessage, $"Invalid InvestmentAmount for plan {testPlan} is not occurred");
        }


        [Test]
        public void InvalidRetirementAgeAgeRangeValidationFailed()
        {
            var quoteRequest = new QuoteRequestBuilder().
                WithSilverPlanQuote().
                WithInvalidRetirementAge().Build();

            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(quoteRequest, new ValidationContext(quoteRequest), result, true);

            Assert.IsFalse(isValid);

            Assert.AreEqual("RetirementAge should be between (60,75)", result[0].ErrorMessage, "RetirementAge Range validation is not occurred");

        }

        [Test]
        [TestCase(Constants.PensionPlan.Silver.Name)]
        [TestCase(Constants.PensionPlan.Gold.Name)]        
        public void PlanWithIneligibleRetirementAgeValidationFailed(string testPlan)
        {
            var plan = PensionPlanFactory.GetPlan(testPlan);

            var quoteRequest = new QuoteRequestBuilder().
                WithPlanQuote(testPlan).
                WithRetirementAge(plan.MinimumRetirementAge - 1).Build();

            var result = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(quoteRequest, new ValidationContext(quoteRequest), result, true);

            Assert.IsFalse(isValid);

            var expectedMessage = $"RetirementAge should be at least {plan.MinimumRetirementAge} for plan {quoteRequest.PensionPlan}";

            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(expectedMessage, result[0].ErrorMessage, $"Invalid RetirementAge for plan {testPlan} is not occurred");
        }

        [Test]
        public void PlatinumPlanWithIneligibleRetirementAgeValidationFailed2ErrorMessages()
        {
            var quoteRequest = new QuoteRequestBuilder().
                WithPlatinumPlanQuote().
                WithIneligiblePlatinumPlanRetirementAge().
                Build();

            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(quoteRequest, new ValidationContext(quoteRequest), results, true);

            Assert.IsFalse(isValid);

            var expectedMessages = new string[] {
                    $"RetirementAge should be at least {PensionPlan.Platinum.MinimumRetirementAge} for plan {quoteRequest.PensionPlan}",
                    $"RetirementAge should be between (60,75)" };

            Assert.AreEqual(expectedMessages.Count(), results.Count);

            Assert.IsTrue(results.All(e => expectedMessages.Contains(e.ErrorMessage)), "Expected validation messages not occurred");

        }

    }
}
