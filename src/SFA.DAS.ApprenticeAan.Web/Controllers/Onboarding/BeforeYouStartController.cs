using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Route("onboarding/before-you-start", Name = RouteNames.Onboarding.BeforeYouStart)]
public class BeforeYouStartController : OnboardingControllerBase
{
    public const string ViewPath = "~/Views/Onboarding/BeforeYouStart.cshtml";

    private readonly ApplicationConfiguration _applicationConfiguration;

    public BeforeYouStartController(ISessionService sessionService, ApplicationConfiguration applicationConfiguration) : base(sessionService)
    {
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
        SessionService.Set(new OnboardingSessionModel());
        return Ok();
    }
}
