using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

//TODO remove ExcludeFromCodeCoverage on proper implementation
//This is initial implementation just to get api client working example
//remove this comments once proper implementation is done
[ExcludeFromCodeCoverage]
[Route("onboarding/regions", Name = RouteNames.Onboarding.Regions)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
public class RegionsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/Regions.cshtml";
    private readonly IRegionService _regionService;
    private readonly ISessionService _sessionService;

    public RegionsController(ISessionService sessionService, IRegionService regionService)
    {
        _sessionService = sessionService;
        _regionService = regionService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var model = new RegionsViewModel
        {
            BackLink = Url.RouteUrl(RouteNames.Onboarding.TermsAndConditions)!,
            Regions = await _regionService.GetRegions()
        };

        return View(ViewPath, model);
    }

    [HttpPost]
    //TODO: Variables not being passed to controller from View.
    public IActionResult Post(string value)
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        sessionModel.RegionId = Convert.ToInt16(value);
        _sessionService.Set(sessionModel);
        return Ok(sessionModel);

        //TODO: Redirect to "Why do you want to join the network" page, when developed
        //return RedirectToRoute(RouteNames.Onboarding.TermsAndConditions);

    }
}
