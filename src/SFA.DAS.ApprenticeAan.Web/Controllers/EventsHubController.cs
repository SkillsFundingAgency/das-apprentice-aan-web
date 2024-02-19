using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("events-hub", Name = SharedRouteNames.EventsHub)]
public class EventsHubController(IOuterApiClient apiClient, ISessionService sessionService) : Controller
{
    private readonly IOuterApiClient _apiClient = apiClient;
    private readonly ISessionService _sessionService = sessionService;

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int? month, [FromQuery] int? year, CancellationToken cancellationToken)
    {
        month ??= DateTime.Today.Month;
        year ??= DateTime.Today.Year;

        // throws ArgumentOutOfRangeException if the month is invalid, which will navigate user to an error page
        var firstDayOfTheMonth = new DateOnly(year.GetValueOrDefault(), month.GetValueOrDefault(), 1);
        var lastDayOfTheMonth = new DateOnly(firstDayOfTheMonth.Year, firstDayOfTheMonth.Month, DateTime.DaysInMonth(firstDayOfTheMonth.Year, firstDayOfTheMonth.Month));

        var response = await _apiClient.GetAttendances(_sessionService.GetMemberId(), firstDayOfTheMonth.ToApiString(), lastDayOfTheMonth.ToApiString(), cancellationToken);

        EventsHubViewModel model = new(firstDayOfTheMonth, Url, GetAppointments(response.Attendances), () => Url.RouteUrl(SharedRouteNames.NetworkEvents)!);
        return View(model);
    }

    private List<Appointment> GetAppointments(List<Attendance> attendances)
    {
        List<Appointment> appointments = [];
        foreach (Attendance attendance in attendances)
        {
            appointments.Add(attendance.ToAppointment(Url));
        }
        return appointments;
    }
}
