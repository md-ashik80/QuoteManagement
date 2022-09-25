namespace ModernQuote.Api.Constants
{
    public static class Messages
    {
        public const string NameExceedsCharacters = "Name should not exceeds 30 characters";

        public const string PastDateOfBirthRequired = "Date of Birth should be in past.";

        public const string MobileNumberIsInvalid = "MobileNumber should be 10 digits.";

        public const string RetirementAgeOrPlanIsInvalid = "RetirementAge or Plan is Invalid";

        public const string RetirementAgeRangeIsInvalid = "RetirementAge should be between (60,75)";

        public const string RetirementAgeLimitOfPlan = "RetirementAge should be at least {0} for plan {1}";

        public const string InvestmentAmountLimitOfPlan = "InvestmentAmount should be at least {0} for plan {1}";

        public const string InvestmentAmountOrPlanIsInvalid = "InvestmentAmount or Plan is Invalid";

}

    public static class Sex
    {
        public const string Male = "Male";

        public const string Female = "Female";
    }

    public static class PensionPlan
    {
        public static class Silver
        {
            public const string Name = "PensionSilver";

            public const int MinimumRetirementAge = 65;

            public const decimal MinimumInvestmentAmount = 100000;

            public const decimal Factor = 0.02m;
        }

        public static class Gold
        {
            public const string Name = "PensionGold";

            public const int MinimumRetirementAge = 63;

            public const decimal MinimumInvestmentAmount = 300000;

            public const decimal Factor = 0.04m;
        }

        public static class Platinum
        {
            public const string Name = "PensionPlatinum";

            public const int MinimumRetirementAge = 60;

            public const decimal MinimumInvestmentAmount = 500000;

            public const decimal Factor = 0.06m;
        }
    }
}
