using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;
namespace SFA.DAS.ApprenticeAan.Web.Controllers.EventNotificationSettings;

[Authorize]
[Route("event-notification-settings", Name = RouteNames.EventNotificationSettings.UpcomingEventsNotifications)]
public class EventNotificationSettingsController : Controller
{
    public IActionResult Index([FromRoute] string employerAccountId)
    {
        var model = new EventNotificationSettingsViewModel()
        {

        };

        return View(model);
    }
}