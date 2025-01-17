using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("profile-settings", Name = SharedRouteNames.ProfileSettings)]
public class ProfileSettingsController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        ProfileSettingsViewModel model = new()
        {
            YourAmbassadorProfileUrl = Url.RouteUrl(SharedRouteNames.YourAmbassadorProfile)!,
            UpcomingEventsNotificationsUrl = Url.RouteUrl(RouteNames.EventNotificationSettings.UpcomingEventsNotifications)!,
            LeaveTheNetworkUrl = Url.RouteUrl(SharedRouteNames.LeaveTheNetwork)!
        };

        return View(model);
    }
}
