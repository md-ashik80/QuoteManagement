namespace ModernQuote.Api.Rules
{
    public interface IPensionPlan
    {
        string Name { get;}

        int MinimumRetirementAge { get; }

        decimal MinimumInvestmentAmount { get; }

        decimal Factor { get; }
    }
}
