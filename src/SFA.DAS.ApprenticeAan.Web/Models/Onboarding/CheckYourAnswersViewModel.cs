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
    
    public string AreasOfInterestChangeLink { get; }
    public string AreasOfInterest { get; }


    public CheckYourAnswersViewModel(IUrlHelper url, OnboardingSessionModel sessionModel)
    {
        JobTitleChangeLink = url.RouteUrl(@RouteNames.Onboarding.CurrentJobTitle)!;
        JobTitle = sessionModel.GetProfileValue(ProfileDataId.JobTitle)!;

        RegionChangeLink = url.RouteUrl(@RouteNames.Onboarding.Regions)!;
        Region = sessionModel.RegionName!;
        
        
        ReasonForJoiningTheNetworkChangeLink = url.RouteUrl(@RouteNames.Onboarding.ReasonToJoin)!;
        ReasonForJoiningTheNetwork = sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork!;
        
        AreasOfInterestChangeLink = url.RouteUrl(@RouteNames.Onboarding.AreasOfInterest)!;
        AreasOfInterest = GetAreasOfInterest(sessionModel)!;

    private string? GetAreasOfInterest(OnboardingSessionModel sessionModel)
    {
        var EventsAndPromotions = sessionModel.ProfileData.Where(x => (x.Category == "Events" || x.Category == "Promotions") && x.Value != null)
            .OrderBy(x => x.Id).Select(x => x.Description);

        var areaOfInterests = string.Join(", ", EventsAndPromotions);

        return areaOfInterests;
    }
}