namespace ModernQuote.Api.Rules
{
    public class PlatinumPlan : IPensionPlan
    {
        public string Name => Constants.PensionPlan.Platinum.Name;

        public int MinimumRetirementAge => Constants.PensionPlan.Platinum.MinimumRetirementAge;

        public decimal MinimumInvestmentAmount => Constants.PensionPlan.Platinum.MinimumInvestmentAmount;

        public decimal Factor => Constants.PensionPlan.Platinum.Factor;
    }
}
