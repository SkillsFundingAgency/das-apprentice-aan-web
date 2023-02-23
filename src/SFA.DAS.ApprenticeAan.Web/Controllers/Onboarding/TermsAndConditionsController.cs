using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Route("onboarding/terms-and-conditions", Name = RouteNames.Onboarding.TermsAndConditions)]
public class TermsAndConditionsController : OnboardingControllerBase
{
    public const string ViewPath = "~/Views/Onboarding/TermsAndConditions.cshtml";

    private readonly ApplicationConfiguration _applicationConfiguration;

    public TermsAndConditionsController(ISessionService sessionService, ApplicationConfiguration applicationConfiguration) : base(sessionService)
    {
        _applicationConfiguration = applicationConfiguration;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var model = new TermsAndConditionsViewModel()
        {
            BackLink = Url.RouteUrl(RouteNames.Onboarding.BeforeYouStart)!
        };
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post()
    {
        var sessionModel = SessionService.Get<OnboardingSessionModel>();
        sessionModel.HasAcceptedTermsAndConditions = true;
        SessionService.Set(sessionModel);

        return Ok();
    }
}
