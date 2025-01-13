using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/select-notifications", Name = RouteNames.Onboarding.SelectNotificationEvents)]
[HideNavigationBar(true, true)]
public class SelectNotificationsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/SelectNotifications.cshtml";
    private readonly ISessionService _sessionService;

    public SelectNotificationsController(
        ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();
        return View(ViewPath);
    }
}
