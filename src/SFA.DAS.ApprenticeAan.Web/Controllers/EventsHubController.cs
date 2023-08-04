using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("events-hub", Name = SharedRouteNames.EventsHub)]
public class EventsHubController : Controller
{
    private readonly IOuterApiClient _apiClient;

    public EventsHubController(IOuterApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int? month, [FromQuery] int? year, CancellationToken cancellationToken)
    {
        month = month ?? DateTime.Today.Month;
        year = year ?? DateTime.Today.Year;

        // throws ArgumentOutOfRangeException if the month is invalid, which will navigate user to an error page
        var firstDayOfTheMonth = new DateOnly(year.GetValueOrDefault(), month.GetValueOrDefault(), 1);
        var lastDayOfTheMonth = new DateOnly(firstDayOfTheMonth.Year, firstDayOfTheMonth.Month, DateTime.DaysInMonth(firstDayOfTheMonth.Year, firstDayOfTheMonth.Month));

        var response = await _apiClient.GetAttendances(User.GetAanMemberId(), firstDayOfTheMonth.ToApiString(), lastDayOfTheMonth.ToApiString(), cancellationToken);

        EventsHubViewModel model = new(firstDayOfTheMonth, Url, GetAppointments(response.Attendances));
        return View(model);
    }

    private List<Appointment> GetAppointments(List<Attendance> attendances)
    {
        List<Appointment> appointments = new();
        foreach (Attendance attendance in attendances)
        {
            appointments.Add(attendance.ToAppointment(Url));
        }
        return appointments;
    }
}
