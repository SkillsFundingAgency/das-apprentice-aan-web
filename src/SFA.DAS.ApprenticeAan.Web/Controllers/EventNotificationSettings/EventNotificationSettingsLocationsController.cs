using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;
using SFA.DAS.ApprenticeAan.Web.Orchestrators;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.EventNotificationSettings
{
    [Authorize]
    [Route("event-notification-settings/locations", Name = RouteNames.EventNotificationSettings.SettingsLocations)]
    public class EventNotificationSettingsLocationsController(
        IEventNotificationSettingsOrchestrator orchestrator,
        ISessionService sessionService)
        : Controller
    {
        public const string ViewPath = "~/Views/EventNotificationSettings/Locations.cshtml";

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var sessionModel = sessionService.Get<NotificationSettingsSessionModel>();

            return View(ViewPath);
        }
    }
}
