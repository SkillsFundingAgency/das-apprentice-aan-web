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
    public async Task<IActionResult> Post()
    {
        var profiles = await _profileService.GetProfilesByUserType("apprentice");
        var sessionModel = new OnboardingSessionModel();
        sessionModel.ProfileData = profiles.Select(p => (ProfileModel)p).ToList();
        _sessionService.Set(sessionModel);
        return RedirectToRoute(RouteNames.Onboarding.TermsAndConditions);
    }
}