using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;

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
            YourNetworkProfileUrl = Url.RouteUrl(SharedRouteNames.YourNetworkProfile)!,
            LeaveTheNetworkUrl = Url.RouteUrl(SharedRouteNames.LeaveTheNetwork)!
        };

        return View(model);
    }
}
