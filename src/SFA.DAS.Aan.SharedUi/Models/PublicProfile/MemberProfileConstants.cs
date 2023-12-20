using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aan.SharedUi.Models.PublicProfile;

[ExcludeFromCodeCoverage]
public static class MemberProfileConstants
{
    public const string RegionalChair = "Regional chair";

    public static class MemberProfileDescription
    {
        public const string Biography = "Biography";
        public const string JobTitle = "Job title";
        public const string LinkedIn = "LinkedIn";
    }

    public static class AreasOfInterest
    {
        public readonly static (string Title, string Category) ApprenticeAreasOfInterestFirstSection = ("Events:", "Events");
        public readonly static (string Title, string Category) ApprenticeAreasOfInterestSecondSection = ("Promoting the network:", "Promotions");
        public readonly static (string Title, string Category) EmployerAreasOfInterestFirstSection = ("Why I wanted to join the network:", "ReasonToJoin");
        public readonly static (string Title, string Category) EmployerAreasOfInterestSecondSection = ("What support I need from the network:", "Support");
    }
}
