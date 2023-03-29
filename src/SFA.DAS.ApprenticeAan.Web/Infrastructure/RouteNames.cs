using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Web.Infrastructure;

[ExcludeFromCodeCoverage]
public static class RouteNames
{
    public static class Onboarding
    {
        public const string BeforeYouStart = nameof(BeforeYouStart);
        public const string TermsAndConditions = nameof(TermsAndConditions);
        public const string LineManager = nameof(LineManager);
        public const string Regions = nameof(Regions);
        public const string CurrentJobTitle = nameof(CurrentJobTitle);
        public const string JoinTheNetwork = nameof(JoinTheNetwork);
        public const string NameOfEmployer = nameof(NameOfEmployer);
        public const string EmployerDetails = nameof(EmployerDetails);
        public const string ReasonToJoinTheNetwork = nameof(ReasonToJoinTheNetwork);
        public const string AreasOfInterest = nameof(AreasOfInterest);
        public const string CheckYourAnswers = nameof(CheckYourAnswers);
        public const string PreviousEngagement = nameof(PreviousEngagement);
    }
}