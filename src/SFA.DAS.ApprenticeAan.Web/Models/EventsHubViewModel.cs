using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class EventsHubViewModel
{
    private readonly IUrlHelper _urlHelper;
    public string AllNetworksUrl { get; init; }

    public CalendarViewModel Calendar { get; init; }

    public EventsHubViewModel(DateOnly firstDayOfTheMonth, IUrlHelper urlHelper, List<Attendance> attendances)
    {
        _urlHelper = urlHelper;
        AllNetworksUrl = urlHelper.RouteUrl(RouteNames.NetworkEvents)!;
        Calendar = new(firstDayOfTheMonth, DateOnly.FromDateTime(DateTime.Today), GetAppointments(attendances));
        Calendar.PreviousMonthLink = urlHelper.RouteUrl(RouteNames.EventsHub, new { firstDayOfTheMonth.AddMonths(-1).Month, firstDayOfTheMonth.AddMonths(-1).Year })!;
        Calendar.NextMonthLink = _urlHelper.RouteUrl(RouteNames.EventsHub, new { firstDayOfTheMonth.AddMonths(1).Month, firstDayOfTheMonth.AddMonths(1).Year })!;
    }

    private List<Appointment> GetAppointments(List<Attendance> attendances)
    {
        List<Appointment> appointments = new();
        foreach (Attendance attendance in attendances)
        {
            appointments.Add(attendance.ToAppointment(_urlHelper));
        }
        return appointments;
    }
}
