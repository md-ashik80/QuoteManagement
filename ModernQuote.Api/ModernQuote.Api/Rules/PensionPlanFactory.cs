namespace ModernQuote.Api.Rules
{
    public static class PensionPlanFactory
    {
        public static IPensionPlan GetPlan(string plan)
        {
            switch( plan )
            {
                case Constants.PensionPlan.Silver.Name:
                    return new SilverPlan();

                case Constants.PensionPlan.Gold.Name:
                    return new GoldPlan();

                case Constants.PensionPlan.Platinum.Name:
                    return new PlatinumPlan();

                default:
                    return null;
            }
        }
    }
}
