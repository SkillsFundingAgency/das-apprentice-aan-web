using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;


namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Route("onboarding/before-you-start", Name = RouteNames.Onboarding.BeforeYouStart)]
public class BeforeYouStartController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/BeforeYouStart.cshtml";

    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly ISessionService _sessionService;
    private readonly IProfileService _profileService;

    public BeforeYouStartController(ISessionService sessionService, IProfileService profileService, ApplicationConfiguration applicationConfiguration)
    {
        _sessionService = sessionService;
        _profileService = profileService;
        _applicationConfiguration = applicationConfiguration;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var model = new BeforeYouStartViewModel()
        {
            BackLink = _applicationConfiguration.ApplicationUrls.ApprenticeHomeUrl.ToString()
        };
        return View(ViewPath, model);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync()
    {
        var profiles = await _profileService.GetProfiles();
        _sessionService.Set(new OnboardingSessionModel());
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        sessionModel.ProfileData = profiles.ConvertAll(p => new ProfileModel());
        _sessionService.Set(sessionModel);
        return RedirectToRoute(RouteNames.Onboarding.TermsAndConditions);
    }
}