using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;

namespace SFA.DAS.Aan.SharedUi.Models;

public class EventsHubViewModel
{
    private readonly IUrlHelper _urlHelper;
    public string AllNetworksUrl { get; init; }

    public CalendarViewModel Calendar { get; init; }

    public EventsHubViewModel(DateOnly firstDayOfTheMonth, IUrlHelper urlHelper, List<Appointment> appointments)
    {
        _urlHelper = urlHelper;
        AllNetworksUrl = urlHelper.RouteUrl(SharedRouteNames.NetworkEvents)!;
        Calendar = new(firstDayOfTheMonth, DateOnly.FromDateTime(DateTime.Today), appointments);
        Calendar.PreviousMonthLink = _urlHelper.RouteUrl(SharedRouteNames.EventsHub, new { firstDayOfTheMonth.AddMonths(-1).Month, firstDayOfTheMonth.AddMonths(-1).Year })!;
        Calendar.NextMonthLink = _urlHelper.RouteUrl(SharedRouteNames.EventsHub, new { firstDayOfTheMonth.AddMonths(1).Month, firstDayOfTheMonth.AddMonths(1).Year })!;
    }


}
