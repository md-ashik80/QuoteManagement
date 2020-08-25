using System;
using System.ComponentModel.DataAnnotations;

using NUnit.Framework;
using QuoteManagement.Api.Models;
using QuoteManagement.Api.Validators;

namespace QuoteManagement.Api.Test.Validators
{
    public class PastDateRequiredAttributeTests
    {
        [Test]
        public void IsValidPastDatePassedValidationSuccessful()
        {
            var validator = new PastDateRequiredAttribute();

            var result = validator.IsValid(DateTime.Today.AddYears(-40));

            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidFutureDateFailedWithErrorMessage()
        {
            var validator = new PastDateRequiredAttribute()
            {
                ErrorMessage = Constants.Messages.PastDateOfBirthRequired
            };

            var result = validator.GetValidationResult(DateTime.Today.AddYears(1), new ValidationContext(new QuoteRequest()));

            Assert.AreEqual(Constants.Messages.PastDateOfBirthRequired, result.ErrorMessage);

        }
    }
}
