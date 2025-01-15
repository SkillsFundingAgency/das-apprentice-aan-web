using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/regionalNetwork", Name = RouteNames.Onboarding.RegionalNetwork)]
[HideNavigationBar(true, true)]
public class RegionalNetworkController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/RegionalNetwork.cshtml";
    private readonly ISessionService _sessionService;

    public RegionalNetworkController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        var model = GetViewModel(sessionModel);
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post(RegionalNetworkViewModel submitModel)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        return RedirectToRoute(sessionModel.HasSeenPreview ? RouteNames.Onboarding.CheckYourAnswers : RouteNames.Onboarding.ConfirmDetails);
    }

    private RegionalNetworkViewModel GetViewModel(OnboardingSessionModel sessionModel)
    {
        return new RegionalNetworkViewModel
        {
            SelectedRegion = sessionModel.RegionName!,
            BackLink = Url.RouteUrl(RouteNames.Onboarding.Regions)!
        };
    }
}