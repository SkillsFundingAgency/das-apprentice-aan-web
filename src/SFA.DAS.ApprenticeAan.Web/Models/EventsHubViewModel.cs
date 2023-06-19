using Microsoft.AspNetCore.Mvc;
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
        Calendar = new(firstDayOfTheMonth, urlHelper, DateOnly.FromDateTime(DateTime.Today), GetAppointments(attendances));
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
