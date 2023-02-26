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

    public BeforeYouStartController(ISessionService sessionService, ApplicationConfiguration applicationConfiguration)
    {
        _sessionService = sessionService;
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
    public IActionResult Post()
    {
        _sessionService.Set(new OnboardingSessionModel());
        return RedirectToRoute(RouteNames.Onboarding.TermsAndConditions);
    }
}
