namespace SFA.DAS.ApprenticeAan.Application
{
    public static class Constants
    {
        public static class RegularExpressions
        {
            public const string ExcludedCharactersRegex = @"^[^@#$^=+\\\/<>%]*$";
            public const string PostcodeRegex = @"^[a-zA-z]{1,2}\d[a-zA-z\d]?\s*\d[a-zA-Z]{2}$";
        }
    }
}