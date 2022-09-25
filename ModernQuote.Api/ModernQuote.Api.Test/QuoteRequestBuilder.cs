using ModernQuote.Api.Models;
using System;
using System.Collections.Generic;

namespace ModernQuote.Api.Test
{
    public class QuoteRequestBuilder
    {
        private QuoteRequest quoteRequest = new QuoteRequest();

        public QuoteRequest Request()
        {
            return Create(4, "FQH", "Male", new DateTime(1990, 6, 13));
        }

        public IList<QuoteRequest> ManyRequest()
        {
            return new List<QuoteRequest>
            {
                Create(1, "ABC", "Male", new DateTime(1990, 6, 7)),
                Create(2, "XYZ", "Female", new DateTime(1990, 8, 3)),
                Create(3, "PQR", "Male", new DateTime(1990, 11, 24))
            };
        }

        public QuoteRequest Create(int id, string name, string sex, DateTime dateOfBirth)
        {
            return new QuoteRequest
            {
                Id = id,
                Name = name,
                Sex = sex,
                DateOfBirth = dateOfBirth
            };
        }

        public QuoteRequest Build()
        {
            return quoteRequest;
        }

        public QuoteRequestBuilder WithSilverPlanQuote()
        {
            quoteRequest.Name = "ABC BCD";
            quoteRequest.Sex = Constants.Sex.Male;
            quoteRequest.DateOfBirth = DateTime.Today.AddYears(-40);
            quoteRequest.Email = "moh.sha@test.com";
            quoteRequest.MobileNumber = "9213030307";
            quoteRequest.QuoteDate = DateTime.Today;

            return WithSilverPlan().
            WithEligibleSilverPlanInvestmentAmount().
            WithEligibleSilverPlanRetirementAge();
        }

        public QuoteRequestBuilder WithGoldPlanQuote()
        {
            return this.WithSilverPlanQuote().
                WithGoldPlan().
                WithEligibleGoldPlanInvestmentAmount().
                WithEligibleGoldPlanRetirementAge();
        }

        public QuoteRequestBuilder WithPlatinumPlanQuote()
        {
            return this.WithSilverPlanQuote().
                WithPlatinumPlan().
                WithEligiblePlatinumPlanInvestmentAmount().
                WithEligiblePlatinumPlanRetirementAge();
        }

        public QuoteRequestBuilder WithPlanQuote(string plan)
        {
            return plan == Constants.PensionPlan.Silver.Name ?
                WithSilverPlanQuote() : plan == Constants.PensionPlan.Gold.Name ?
                WithGoldPlanQuote() : plan == Constants.PensionPlan.Platinum.Name ?
                WithPlatinumPlanQuote() : null;
        }

        public QuoteRequestBuilder WithExceedLengthName()
        {
            quoteRequest.Name = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFG";

            return this;
        }

        public QuoteRequestBuilder WithInvalidSex()
        {
            quoteRequest.Sex = "None";

            return this;
        }

        public QuoteRequestBuilder WithInvalidDateOfBirth()
        {
            quoteRequest.DateOfBirth = DateTime.Today.AddDays(10);

            return this;
        }

        public QuoteRequestBuilder WithInvalidEmail()
        {
            quoteRequest.Email = "ass222";

            return this;
        }

        public QuoteRequestBuilder WithInvalidMobileNumber()
        {
            quoteRequest.MobileNumber = "9876";

            return this;
        }

        public QuoteRequestBuilder WithPlan(string plan)
        {
            quoteRequest.PensionPlan = plan;

            return this;
        }

        public QuoteRequestBuilder WithSilverPlan() => WithPlan(Constants.PensionPlan.Silver.Name);

        public QuoteRequestBuilder WithGoldPlan() => WithPlan(Constants.PensionPlan.Gold.Name);

        public QuoteRequestBuilder WithPlatinumPlan() => WithPlan(Constants.PensionPlan.Platinum.Name);

        public QuoteRequestBuilder WithUnknownPlan() => WithPlan("Unknown");

        public QuoteRequestBuilder WithInvestmentAmount(decimal investmentAmount)
        {
            quoteRequest.InvestmentAmount = investmentAmount;

            return this;
        }

        public QuoteRequestBuilder WithEligibleSilverPlanInvestmentAmount()
            => WithInvestmentAmount(Constants.PensionPlan.Silver.MinimumInvestmentAmount + 1000);

        public QuoteRequestBuilder WithIneligibleSilverPlanInvestmentAmount()
           => WithInvestmentAmount(Constants.PensionPlan.Silver.MinimumInvestmentAmount - 1000);

        public QuoteRequestBuilder WithEligibleGoldPlanInvestmentAmount()
            => WithInvestmentAmount(Constants.PensionPlan.Gold.MinimumInvestmentAmount + 1000);

        public QuoteRequestBuilder WithIneligibleGoldPlanInvestmentAmount()
           => WithInvestmentAmount(Constants.PensionPlan.Gold.MinimumInvestmentAmount - 1000);

        public QuoteRequestBuilder WithEligiblePlatinumPlanInvestmentAmount()
            => WithInvestmentAmount(Constants.PensionPlan.Platinum.MinimumInvestmentAmount + 1000);

        public QuoteRequestBuilder WithIneligiblePlatinumPlanInvestmentAmount()
           => WithInvestmentAmount(Constants.PensionPlan.Platinum.MinimumInvestmentAmount - 1000);

        public QuoteRequestBuilder WithRetirementAge(int RetirementAge)
        {
            quoteRequest.RetirementAge = RetirementAge;

            return this;
        }

        public QuoteRequestBuilder WithInvalidRetirementAge() => WithRetirementAge(55);

        public QuoteRequestBuilder WithEligibleSilverPlanRetirementAge()
            => WithRetirementAge(Constants.PensionPlan.Silver.MinimumRetirementAge + 10);

        public QuoteRequestBuilder WithIneligibleSilverPlanRetirementAge()
           => WithRetirementAge(Constants.PensionPlan.Silver.MinimumRetirementAge - 10);

        public QuoteRequestBuilder WithEligibleGoldPlanRetirementAge()
            => WithRetirementAge(Constants.PensionPlan.Gold.MinimumRetirementAge + 10);

        public QuoteRequestBuilder WithIneligibleGoldPlanRetirementAge()
           => WithRetirementAge(Constants.PensionPlan.Gold.MinimumRetirementAge - 10);

        public QuoteRequestBuilder WithEligiblePlatinumPlanRetirementAge()
            => WithRetirementAge(Constants.PensionPlan.Platinum.MinimumRetirementAge + 10);

        public QuoteRequestBuilder WithIneligiblePlatinumPlanRetirementAge()
           => WithRetirementAge(Constants.PensionPlan.Platinum.MinimumRetirementAge - 10);

        public QuoteRequestBuilder WithPastQuoteDate()
        {
            quoteRequest.QuoteDate = DateTime.Today.AddDays(-10);

            return this;
        }

        public QuoteRequestBuilder WithMaturityAmount(decimal amount)
        {
            quoteRequest.MaturityAmount = amount;

            return this;
        }

    }
}
