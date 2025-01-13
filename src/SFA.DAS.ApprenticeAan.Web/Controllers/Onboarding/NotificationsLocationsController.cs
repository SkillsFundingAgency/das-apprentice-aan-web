using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/notifications-locations", Name = RouteNames.Onboarding.NotificationsLocations)]
[HideNavigationBar(true, true)]
public class NotificationsLocationsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/NotificationsLocations.cshtml";
    private readonly ISessionService _sessionService;
    public NotificationsLocationsController(ISessionService sessionService, IValidator<ReceiveNotificationsSubmitModel> validator)
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

