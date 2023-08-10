using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;

namespace SFA.DAS.Aan.SharedUi.Models;

public class EventsHubViewModel
{
    public string AllNetworksUrl { get; init; }

    public CalendarViewModel Calendar { get; init; }

    public EventsHubViewModel(DateOnly firstDayOfTheMonth, IUrlHelper urlHelper, List<Appointment> appointments)
    {
        AllNetworksUrl = urlHelper.RouteUrl(SharedRouteNames.NetworkEvents)!;
        Calendar = new(firstDayOfTheMonth, DateOnly.FromDateTime(DateTime.Today), appointments);
        Calendar.PreviousMonthLink = urlHelper.RouteUrl(SharedRouteNames.EventsHub, new { firstDayOfTheMonth.AddMonths(-1).Month, firstDayOfTheMonth.AddMonths(-1).Year })!;
        Calendar.NextMonthLink = urlHelper.RouteUrl(SharedRouteNames.EventsHub, new { firstDayOfTheMonth.AddMonths(1).Month, firstDayOfTheMonth.AddMonths(1).Year })!;
    }
}
