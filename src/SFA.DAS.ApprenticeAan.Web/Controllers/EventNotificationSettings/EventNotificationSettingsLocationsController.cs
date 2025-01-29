using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;
using SFA.DAS.ApprenticeAan.Web.Orchestrators;
using SFA.DAS.ApprenticeAan.Web.Orchestrators.Shared;
using SFA.DAS.Validation.Mvc.Filters;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.EventNotificationSettings
{
    [Authorize]
    [Route("event-notification-settings/locations", Name = RouteNames.EventNotificationSettings.NotificationLocations)]
    public class EventNotificationSettingsLocationsController(
        INotificationsLocationsOrchestrator orchestrator,
        ISessionService sessionService,
        IOuterApiClient apiClient,
        IEventNotificationSettingsOrchestrator settingsOrchestrator)
        : Controller
    {
        public const string ViewPath = "~/Views/EventNotificationSettings/Locations.cshtml";

        [HttpGet]
        [ValidateModelStateFilter]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var sessionModel = sessionService.Get<NotificationSettingsSessionModel?>();

            if (sessionModel == null)
            {
                return RedirectToRoute(RouteNames.EventNotificationSettings.Settings);
            }

            var viewModel = orchestrator.GetViewModel<NotificationsLocationsViewModel>(sessionModel, ModelState);
            viewModel.BackLink = Url.RouteUrl(RouteNames.EventNotificationSettings.Settings);

            return View(ViewPath, viewModel);
        }

        [HttpPost]
        [ValidateModelStateFilter]
        public async Task<IActionResult> Index(NotificationsLocationsSubmitModel submitModel)
        {
            var result = await orchestrator.ApplySubmitModel<NotificationSettingsSessionModel>(
                submitModel,
                ModelState,
                async (location) => await apiClient.GetOnboardingNotificationsLocations(location)
            );

            if (result == NotificationsLocationsOrchestrator.RedirectTarget.NextPage)
            {
                await SaveSettings(submitModel);
                return new RedirectToRouteResult(RouteNames.EventNotificationSettings.Settings, new{});
            }

            return result switch
            {
                NotificationsLocationsOrchestrator.RedirectTarget.Disambiguation
                    => new RedirectToRouteResult(
                        RouteNames.EventNotificationSettings.SettingsNotificationLocationDisambiguation,
                        new { submitModel.Radius, submitModel.Location }),
                NotificationsLocationsOrchestrator.RedirectTarget.Self => new RedirectToRouteResult(RouteNames
                    .EventNotificationSettings.NotificationLocations, new {}),
                _ => throw new InvalidOperationException("Unexpected redirect target from ApplySubmitModel"),
            };
        }

        private async Task SaveSettings(NotificationsLocationsSubmitModel submitModel)
        {
            var memberId = sessionService.GetMemberId();
            var sessionModel = sessionService.Get<NotificationSettingsSessionModel>();
            await settingsOrchestrator.SaveSettings(memberId, sessionModel);
        }
    }
}
