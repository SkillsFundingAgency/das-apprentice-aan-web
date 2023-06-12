using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class EventsHubViewModel
{
    public string AllNetworksUrl { get; init; }

    public CalendarViewModel Calendar { get; init; }

    public EventsHubViewModel(DateOnly firstDayOfTheMonth, IUrlHelper urlHelper)
    {
        AllNetworksUrl = urlHelper.RouteUrl(RouteNames.NetworkEvents)!;
        Calendar = new(firstDayOfTheMonth, urlHelper, DateOnly.FromDateTime(DateTime.Today));
    }
}
