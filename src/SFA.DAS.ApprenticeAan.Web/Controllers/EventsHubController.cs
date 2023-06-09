using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("events-hub", Name = RouteNames.EventsHub)]
public class EventsHubController : Controller
{
    public IActionResult Index()
    {
        EventsHubViewModel model = new()
        {
            AllNetworksUrl = Url.RouteUrl(RouteNames.NetworkEvents)!
        };
        return View(model);
    }
}
