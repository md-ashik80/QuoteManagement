namespace QuoteManagement.Api.Rules
{
    public class GoldPlan : IPensionPlan
    {
        public string Name => Constants.PensionPlan.Gold.Name;

        public int MinimumRetirementAge => Constants.PensionPlan.Gold.MinimumRetirementAge;

        public decimal MinimumInvestmentAmount => Constants.PensionPlan.Gold.MinimumInvestmentAmount;

        public decimal Factor => Constants.PensionPlan.Gold.Factor;
    }
}
