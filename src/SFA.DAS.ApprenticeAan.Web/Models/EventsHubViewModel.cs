using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class EventsHubViewModel
{
    public string AllNetworksUrl { get; init; }

    public CalendarViewModel Calendar { get; init; }

    public EventsHubViewModel(int month, int year, IUrlHelper urlHelper)
    {
        AllNetworksUrl = urlHelper.RouteUrl(RouteNames.NetworkEvents)!;
        Calendar = new(month, year, urlHelper, DateOnly.FromDateTime(DateTime.Today));
    }
}
