namespace SFA.DAS.ApprenticeAan.Application
{
    public static class Constants
    {
        public static class RegularExpressions
        {
            public const string ExcludedCharactersRegex = @"^[^@#$^=+\\\/<>%]*$";
            public const string PostcodeRegex = @"^[a-zA-z]{1,2}\d[a-zA-z\d]?\s*\d[a-zA-Z]{2}$";
            public const string AlphaNumericCharactersRegex = "^[a-zA-Z0-9]*$";
        }

        public static class NotificationTemplateNames
        {
            public const string AANApprenticeOnboarding = nameof(AANApprenticeOnboarding);
            public const string AANApprenticeEventSignup = nameof(AANApprenticeEventSignup);
            public const string AANApprenticeEventCancel = nameof(AANApprenticeEventCancel);
            public const string AANApprenticeWithdrawal = nameof(AANApprenticeWithdrawal);
            public const string AANAdminEventUpdate = nameof(AANAdminEventUpdate);
            public const string AANAdminEventCancel = nameof(AANAdminEventCancel);
            public const string AANIndustryAdvice = nameof(AANIndustryAdvice);
            public const string AANAskForHelp = nameof(AANAskForHelp);
            public const string AANRequestCaseStudy = nameof(AANRequestCaseStudy);
            public const string AANGetInTouch = nameof(AANGetInTouch);
        }
    }
}