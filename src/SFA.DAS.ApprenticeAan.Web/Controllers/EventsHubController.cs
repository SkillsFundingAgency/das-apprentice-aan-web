using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("events-hub", Name = RouteNames.EventsHub)]
public class EventsHubController : Controller
{
    public IActionResult Index([FromQuery] int? month, [FromQuery] int? year, CancellationToken cancellationToken)
    {
        month = month ?? DateTime.Today.Month;
        year = year ?? DateTime.Today.Year;

        var d = new DateOnly(year.GetValueOrDefault(), month.GetValueOrDefault(), 1);

        EventsHubViewModel model = new(d.Month, d.Year, Url);
        return View(model);
    }
}
