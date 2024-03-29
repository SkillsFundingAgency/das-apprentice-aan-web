﻿using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;

namespace SFA.DAS.Aan.SharedUi.Models;

public class EventsHubViewModel : INetworkHubLink
{
    public string AllNetworksUrl { get; init; }

    public CalendarViewModel Calendar { get; init; }

    public string? NetworkHubLink { get; set; }

    public EventsHubViewModel(DateOnly firstDayOfTheMonth, IUrlHelper urlHelper, List<Appointment> appointments, Func<string> getNetworkEventsUrl)
    {
        AllNetworksUrl = getNetworkEventsUrl();
        Calendar = new(firstDayOfTheMonth, DateOnly.FromDateTime(DateTime.Today), appointments);
        Calendar.PreviousMonthLink = urlHelper.RouteUrl(SharedRouteNames.EventsHub, new { firstDayOfTheMonth.AddMonths(-1).Month, firstDayOfTheMonth.AddMonths(-1).Year })!;
        Calendar.NextMonthLink = urlHelper.RouteUrl(SharedRouteNames.EventsHub, new { firstDayOfTheMonth.AddMonths(1).Month, firstDayOfTheMonth.AddMonths(1).Year })!;
    }
}
