using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Route("onboarding/terms-and-conditions", Name = RouteNames.Onboarding.TermsAndConditions)]
public class TermsAndConditionsController : OnboardingControllerBase
{
    public const string ViewPath = "~/Views/Onboarding/TermsAndConditions.cshtml";

    //private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly ILogger<TermsAndConditionsController> _logger;

    public TermsAndConditionsController(ISessionService sessionService, ILogger<TermsAndConditionsController> logger) : base(sessionService)
    {
        _logger = logger;
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
        var (sessionModel, escapeRoute) = GetSessionModelWithEscapeRoute(_logger);
        if (sessionModel == null)
        {
            return escapeRoute!;
        }
        else
        {
            sessionModel.HasAcceptedTermsAndConditions = true;
            SessionService.Set(sessionModel);
            return Ok(sessionModel);
        }
    }
}
