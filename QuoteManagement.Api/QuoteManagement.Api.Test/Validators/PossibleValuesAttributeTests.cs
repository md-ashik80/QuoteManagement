using System;
using System.ComponentModel.DataAnnotations;

using NUnit.Framework;
using QuoteManagement.Api.Models;
using QuoteManagement.Api.Validators;

namespace QuoteManagement.Api.Test.Validators
{
    public class PossibleValuesAttributeTests
    {
        [Test]
        public void IsValidPossibleValuePassedIsSucceeded()
        {
            var validator = new PossibleValuesAttribute
            {
                ExpectedValues = new string[] { Constants.Sex.Male, Constants.Sex.Female },
                FieldName = "Sex"
            };

            Assert.IsTrue(validator.IsValid(Constants.Sex.Male));
        }

        [Test]
        public void IsValidMismatchingValuePassedIsFailed()
        {
            var validator = new PossibleValuesAttribute
            {
                ExpectedValues = new string[] { Constants.Sex.Male, Constants.Sex.Female },
                FieldName = "Sex"
            };

            var result = validator.GetValidationResult("None",
                new ValidationContext(new QuoteRequest()));

            var expectedMessage = string.Format(@"{0} should be one from ({1})", validator.FieldName, String.Join(",", validator.ExpectedValues));

            Assert.AreEqual(expectedMessage, result.ErrorMessage);
        }
    }
}
