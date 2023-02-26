using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Route("onboarding/terms-and-conditions", Name = RouteNames.Onboarding.TermsAndConditions)]
[RequiredSessionModel(typeof(OnboardingSessionModel))]
public class TermsAndConditionsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/TermsAndConditions.cshtml";

    private readonly ILogger<TermsAndConditionsController> _logger;
    private readonly ISessionService _sessionService;

    public TermsAndConditionsController(ISessionService sessionService, ILogger<TermsAndConditionsController> logger)
    {
        _sessionService = sessionService;
        _logger = logger;
    }

    [HttpGet]
    //[RequiredSessionModel(typeof(OnboardingSessionModel))]
    public IActionResult Get()
    {
        var model = new TermsAndConditionsViewModel()
        {
            BackLink = Url.RouteUrl(RouteNames.Onboarding.BeforeYouStart)!
        };
        return View(ViewPath, model);
    }

    [HttpPost]
    //[RequiredSessionModel(typeof(OnboardingSessionModel))]
    public IActionResult Post()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        sessionModel.HasAcceptedTermsAndConditions = true;
        _sessionService.Set(sessionModel);
        return Ok(sessionModel);
    }
}
