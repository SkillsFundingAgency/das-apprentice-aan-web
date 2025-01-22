using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;
using SFA.DAS.Validation.Mvc.Filters;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/notification-disambiguation", Name = RouteNames.Onboarding.NotificationLocationDisambiguation)]
[HideNavigationBar(true, true)]
public class NotificationLocationDisambiguationController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/NotificationLocationDisambiguation.cshtml";
    private readonly ISessionService _sessionService;

    public NotificationLocationDisambiguationController(
        ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    [ValidateModelStateFilter]
    public async Task<IActionResult> Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        return View(ViewPath);
    }
}
