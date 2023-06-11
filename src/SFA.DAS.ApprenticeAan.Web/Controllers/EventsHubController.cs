using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("events-hub", Name = RouteNames.EventsHub)]
public class EventsHubController : Controller
{
    public IActionResult Index([FromQuery] int? month, [FromQuery] int? year, CancellationToken cancellation)
    {

        EventsHubViewModel model = new(month ?? DateTime.Today.Month, year ?? DateTime.Today.Year, Url);
        return View(model);
    }
}
