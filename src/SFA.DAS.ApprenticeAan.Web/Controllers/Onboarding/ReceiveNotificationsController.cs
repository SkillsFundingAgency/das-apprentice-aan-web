using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/receive-notifications", Name = RouteNames.Onboarding.ReceiveNotifications)]
[HideNavigationBar(true, true)]
public class ReceiveNotificationsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/ReceiveNotifications.cshtml";
    private readonly ISessionService _sessionService;
    private readonly IValidator<Models.Shared.ReceiveNotificationsSubmitModel> _validator;
    public ReceiveNotificationsController(ISessionService sessionService, IValidator<Models.Shared.ReceiveNotificationsSubmitModel> validator)
    {
        _sessionService = sessionService;
        _validator = validator;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        var viewModel = new ReceiveNotificationsViewModel
        {
            BackLink = sessionModel.HasSeenPreview
                ? Url.RouteUrl(RouteNames.Onboarding.CheckYourAnswers)!
                : Url.RouteUrl(RouteNames.Onboarding.ReasonToJoin)!,
            ReceiveNotifications = sessionModel.ReceiveNotifications,
        };
        return View(ViewPath, viewModel);
    }

    [HttpPost]
    public IActionResult Post(Models.Shared.ReceiveNotificationsSubmitModel submitModel, CancellationToken cancellationToken)
    {
        var result = _validator.Validate(submitModel);

        if (!result.IsValid)
        {
            var model = new ReceiveNotificationsViewModel
            {
                ReceiveNotifications = submitModel.ReceiveNotifications,
                BackLink = Url.RouteUrl(RouteNames.Onboarding.ReasonToJoin)!,

            };
            result.AddToModelState(ModelState);
            return View(ViewPath, model);
        }

        var sessionModel = _sessionService.Get<OnboardingSessionModel>();

        var originalValue = sessionModel.ReceiveNotifications;
        var newValue = submitModel.ReceiveNotifications!.Value;

        if (!newValue) sessionModel.EventTypes = new List<EventTypeModel>();
        if (!newValue) sessionModel.NotificationLocations = new List<NotificationLocation>();

        sessionModel.ReceiveNotifications = newValue;
        _sessionService.Set(sessionModel);

        var route = sessionModel.HasSeenPreview && newValue == originalValue
            ? RouteNames.Onboarding.CheckYourAnswers
            : newValue
                ? RouteNames.Onboarding.SelectNotificationEvents
                : sessionModel.HasSeenPreview
                    ? RouteNames.Onboarding.CheckYourAnswers
                    : RouteNames.Onboarding.PreviousEngagement;

        return RedirectToRoute(route);
    }
}

