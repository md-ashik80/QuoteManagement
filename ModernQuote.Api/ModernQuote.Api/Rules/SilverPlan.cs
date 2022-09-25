namespace ModernQuote.Api.Rules
{
    public class SilverPlan : IPensionPlan
    {
        public string Name => Constants.PensionPlan.Silver.Name;

        public int MinimumRetirementAge => Constants.PensionPlan.Silver.MinimumRetirementAge;

        public decimal MinimumInvestmentAmount => Constants.PensionPlan.Silver.MinimumInvestmentAmount;

        public decimal Factor => Constants.PensionPlan.Silver.Factor;
    }
}
