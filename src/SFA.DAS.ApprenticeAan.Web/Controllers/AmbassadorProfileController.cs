using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.AmbassadorProfile;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("your-ambassador-profile", Name = SharedRouteNames.YourAmbassadorProfile)]
public class AmbassadorProfileController : Controller
{
    private readonly IOuterApiClient _apiClient;
    private readonly ISessionService _sessionService;

    public AmbassadorProfileController(IOuterApiClient apiClient, ISessionService sessionService)
    {
        _apiClient = apiClient;
        _sessionService = sessionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var profiles = _apiClient.GetProfilesByUserType(MemberUserType.Apprentice.ToString(), cancellationToken);
        var memberProfiles = _apiClient.GetMemberProfile(_sessionService.GetMemberId(), _sessionService.GetMemberId(), false, cancellationToken);
        await Task.WhenAll(profiles, memberProfiles);
        var personalDetails = new PersonalDetailsModel(memberProfiles.Result.FullName, memberProfiles.Result.RegionName, memberProfiles.Result.UserType, Url.RouteUrl(SharedRouteNames.EditPersonalInformation)!, Url.RouteUrl(SharedRouteNames.EditAreaOfInterest)!, Url.RouteUrl(SharedRouteNames.EditContactDetail)!, Url.RouteUrl(SharedRouteNames.MemberProfile, new { id = _sessionService.GetMemberId() })!);
        var apprenticeshipDetails = memberProfiles.Result.Apprenticeship != null ? new ApprenticeshipDetailsModel(memberProfiles.Result.Apprenticeship!.Sector, memberProfiles.Result.Apprenticeship!.Programme, memberProfiles.Result.Apprenticeship!.Level) : null;
        AmbassadorProfileViewModel model = new(personalDetails, memberProfiles.Result.Email, memberProfiles.Result.Profiles, memberProfiles.Result.Preferences, apprenticeshipDetails, profiles.Result.Profiles, Url);
        ViewBag.YourAmbassadorProfileSuccessMessage = TempData[TempDataKeys.YourAmbassadorProfileSuccessMessage];
        return View(model);
    }
}