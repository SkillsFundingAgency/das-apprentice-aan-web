using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.EventNotificationSettings
{
    [Authorize]
    [Route("event-notification-settings/locations/disambiguation", Name = RouteNames.EventNotificationSettings.SettingsNotificationLocationDisambiguation)]
    public class EventNotificationSettingsLocationDisambiguationController : Controller
    {
        public const string ViewPath = "~/Views/EventNotificationSettings/NotificationLocationDisambiguation.cshtml";
        private readonly ISessionService _sessionService;

        public EventNotificationSettingsLocationDisambiguationController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public IActionResult Index()
        {
            var sessionModel = _sessionService.Get<NotificationSettingsSessionModel>();
            return View(ViewPath);
        }
    }
}
