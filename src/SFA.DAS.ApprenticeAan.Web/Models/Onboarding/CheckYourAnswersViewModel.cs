using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

public class CheckYourAnswersViewModel
{
    public string JobTitleChangeLink { get; }
    public string JobTitle { get; }

    public string RegionChangeLink { get; }
    public string Region { get; }

    public string ReasonForJoiningTheNetworkChangeLink { get; }
    public string ReasonForJoiningTheNetwork { get; }

    public CheckYourAnswersViewModel(IUrlHelper url, OnboardingSessionModel sessionModel)
    {
        JobTitleChangeLink = url.RouteUrl(@RouteNames.Onboarding.CurrentJobTitle)!;
        JobTitle = sessionModel.GetProfileValue(ProfileDataId.JobTitle)!;

        RegionChangeLink = url.RouteUrl(@RouteNames.Onboarding.Regions)!;
        Region = sessionModel.RegionName!;
        
        
        ReasonForJoiningTheNetworkChangeLink = url.RouteUrl(@RouteNames.Onboarding.ReasonToJoin)!;
        ReasonForJoiningTheNetwork = sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork!;

    }
}