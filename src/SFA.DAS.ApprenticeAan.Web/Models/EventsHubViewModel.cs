using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
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
        const string style = "app-calendar__event app-calendar__event--";
        List<Appointment> appointments = new();
        foreach (Attendance attendance in attendances)
        {
            var url = GetAppointmentUrl(attendance.CalendarEventId);
            var format = $"{style}{attendance.EventFormat.ToString().ToLower()}";
            var date = DateOnly.FromDateTime(attendance.EventStartDate);
            appointments.Add(new(attendance.EventTitle, url, date, format));
        }
        return appointments;
    }

    private string GetAppointmentUrl(Guid id) => _urlHelper.RouteUrl(RouteNames.NetworkEventDetails, new { Id = id })!;
}
