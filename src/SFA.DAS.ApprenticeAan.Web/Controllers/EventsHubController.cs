using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("events-hub", Name = RouteNames.EventsHub)]
public class EventsHubController : Controller
{
    public async Task<IActionResult> Index([FromQuery] int? month, [FromQuery] int? year, CancellationToken cancellationToken)
    {
        month = month ?? DateTime.Today.Month;
        year = year ?? DateTime.Today.Year;

        // throws ArgumentOutOfRangeException if the month is invalid, which will navigate user to an error page
        var firstDayOfTheMonth = new DateOnly(year.GetValueOrDefault(), month.GetValueOrDefault(), 1);
        var lastDayOfTheMonth = firstDayOfTheMonth.AddDays(DateTime.DaysInMonth(firstDayOfTheMonth.Year, firstDayOfTheMonth.Month));

        //var attendances = await _apiClient.GetAttendances(User.GetAanMemberId(), firstDayOfTheMonth.ToApiString(), lastDayOfTheMonth.ToApiString(), cancellationToken);

        EventsHubViewModel model = new(firstDayOfTheMonth, Url);
        return View(model);
    }
}
