﻿using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Web.Infrastructure;

[ExcludeFromCodeCoverage]
public static class RouteNames
{
    public const string NetworkHub = nameof(NetworkHub);

    public static class Onboarding
    {
        public const string BeforeYouStart = nameof(BeforeYouStart);
        public const string TermsAndConditions = nameof(TermsAndConditions);
        public const string LineManager = nameof(LineManager);
        public const string Regions = nameof(Regions);
        public const string CurrentJobTitle = nameof(CurrentJobTitle);
        public const string EmployerSearch = nameof(EmployerSearch);
        public const string EmployerDetails = nameof(EmployerDetails);
        public const string ReasonToJoin = nameof(ReasonToJoin);
        public const string AreasOfInterest = nameof(AreasOfInterest);
        public const string CheckYourAnswers = nameof(CheckYourAnswers);
        public const string PreviousEngagement = nameof(PreviousEngagement);
    }

    public static class AttendanceConfirmations
    {
        public const string SignUpConfirmation = nameof(SignUpConfirmation);
        public const string CancellationConfirmation = nameof(CancellationConfirmation);
    }
    public const string StubAccountDetailsGet = "account-details-get";
    public const string StubAccountDetailsPost = "account-details-post";
    public const string StubSignedIn = "stub-signedin-get";
}