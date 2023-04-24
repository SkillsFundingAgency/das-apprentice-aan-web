using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

public class CheckYourAnswersViewModel
{
    public string JobTitleChangeLink { get; }
    public string? JobTitle { get; }
    public string RegionChangeLink { get; }
    public string? Region { get; }
    public string ReasonForJoiningTheNetworkChangeLink { get; }
    public string? ReasonForJoiningTheNetwork { get; }
    public string AreasOfInterestChangeLink { get; }
    public List<string> AreasOfInterest { get; }
    public string PreviousEngagementChangeLink { get; }
    public string? PreviousEngagement { get; }

    public CheckYourAnswersViewModel(IUrlHelper url, OnboardingSessionModel sessionModel)
    {
        JobTitleChangeLink = url.RouteUrl(@RouteNames.Onboarding.CurrentJobTitle)!;
        JobTitle = sessionModel.GetProfileValue(ProfileDataId.JobTitle)!;

        RegionChangeLink = url.RouteUrl(@RouteNames.Onboarding.Regions)!;
        Region = sessionModel.RegionName!;

        ReasonForJoiningTheNetworkChangeLink = url.RouteUrl(@RouteNames.Onboarding.ReasonToJoin)!;
        ReasonForJoiningTheNetwork = sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork!;

        AreasOfInterestChangeLink = url.RouteUrl(@RouteNames.Onboarding.AreasOfInterest)!;
        AreasOfInterest = sessionModel.ProfileData.Where(x => (x.Category == Category.Events || x.Category == Category.Promotions) && x.Value != null).Select(x => x.Description).ToList();

        PreviousEngagementChangeLink = url.RouteUrl(@RouteNames.Onboarding.PreviousEngagement)!;
        PreviousEngagement = GetPreviousEngagementValue(sessionModel.GetProfileValue(ProfileDataId.HasPreviousEngagement))!;

    }

    public static string? GetPreviousEngagementValue(string? previousEngagementValue)
    {
        string? resultValue = null;

        if (bool.TryParse(previousEngagementValue, out var result))
            resultValue = result ? "Yes" : "No";

        return resultValue;
    }
}