using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aan.SharedUi.Constants;

[ExcludeFromCodeCoverage]
public static class PreferenceConstants
{
    public static class PreferenceIds
    {
        public const int JobTitle = 1;
        public const int Biography = 2;
        public const int Apprenticeship = 3;
        public const int LinkedIn = 4;
    }

    public static class DisplayValue
    {
        public const string DisplayTagName = "Displayed";
        public const string DisplayTagClass = "govuk-tag";
        public const string HiddenTagName = "Hidden";
        public const string HiddenTagClass = "govuk-tag govuk-tag--blue";
    }
}