using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;
using SFA.DAS.ApprenticeAan.Web.Orchestrators;

namespace SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;

[Authorize]
[Route("event-notification-settings/monthly-notifications", Name = RouteNames.EventNotificationSettings.MonthlyNotifications)]
public class ReceiveNotificationsController(
    IValidator<ReceiveNotificationsSubmitModel> validator,
    IEventNotificationSettingsOrchestrator settingsOrchestrator,
    ISessionService sessionService) : Controller
{
    public const string ViewPath = "~/Views/EventNotificationSettings/ReceiveNotifications.cshtml";

    [HttpGet]
    public IActionResult Get(CancellationToken cancellationToken)
    {
        var sessionModel = sessionService.Get<NotificationSettingsSessionModel?>();

        if (sessionModel == null)
        {
            return RedirectToRoute(RouteNames.EventNotificationSettings.Settings);
        }

        var viewModel = new ReceiveNotificationsViewModel
        {
            BackLink = Url.RouteUrl(RouteNames.EventNotificationSettings.Settings)!,
            ReceiveNotifications = sessionModel.ReceiveNotifications
        };

        return View(ViewPath, viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Post(ReceiveNotificationsSubmitModel submitModel, CancellationToken cancellationToken)
    {
        var result = validator.Validate(submitModel);

        if (!result.IsValid)
        {
            var model = new ReceiveNotificationsViewModel
            {
                ReceiveNotifications = submitModel.ReceiveNotifications,
                BackLink = Url.RouteUrl(RouteNames.EventNotificationSettings.Settings)!,

            };
            result.AddToModelState(ModelState);
            return View(ViewPath, model);
        }

        var memberId = sessionService.GetMemberId();
        var sessionModel = sessionService.Get<NotificationSettingsSessionModel>();

        var originalValue = sessionModel.ReceiveNotifications;
        var newValue = submitModel.ReceiveNotifications!.Value;

        if (!newValue) sessionModel.EventTypes = new List<EventTypeModel>();
        if (!newValue) sessionModel.NotificationLocations = new List<NotificationLocation>();

        sessionModel.ReceiveNotifications = newValue;
        sessionModel.LastPageVisited = RouteNames.EventNotificationSettings.MonthlyNotifications;
        sessionService.Set(sessionModel);

        if (newValue != originalValue && !newValue)
        {
            await settingsOrchestrator.SaveSettings(memberId, sessionModel);
        }

        var route = (newValue != originalValue) && newValue
            ? RouteNames.EventNotificationSettings.EventTypes
                    : RouteNames.EventNotificationSettings.Settings;

        return RedirectToRoute(route);
    }
}
