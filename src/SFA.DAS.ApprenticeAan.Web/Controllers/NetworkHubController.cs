using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-hub", Name = RouteNames.NetworkHub)]
public class NetworkHubController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        NetworkHubViewModel model = new()
        {
            EventsHubUrl = Url.RouteUrl(SharedRouteNames.EventsHub)!,
            NetworkDirectoryUrl = Url.RouteUrl(SharedRouteNames.NetworkDirectory)!,
            ProfileSettingsUrl = Url.RouteUrl(SharedRouteNames.ProfileSettings)!
        };
        return View(model);
    }
}
