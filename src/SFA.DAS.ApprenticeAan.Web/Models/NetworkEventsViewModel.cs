using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Models;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class NetworkEventsViewModel
{
    private readonly IUrlHelper _urlHelper;
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public EventFilters EventFilters { get; set; } = new EventFilters();


    public bool ShowFilterOptions => SearchFilters.Any();
    public List<CalendarEventSummary> CalendarEvents { get; set; } = new List<CalendarEventSummary>();

    public List<SelectedFilter> SearchFilters { get; set; } = new List<SelectedFilter>();

    public NetworkEventsViewModel(IUrlHelper helper)
    {
        _urlHelper = helper;
    }

    public NetworkEventsViewModel(GetCalendarEventsQueryResult result, IUrlHelper urlHelper)
    {
        Page = result.Page;
        PageSize = result.PageSize;
        TotalPages = result.TotalPages;
        TotalCount = result.TotalCount;
        CalendarEvents = result.CalendarEvents.ToList();
        _urlHelper = urlHelper;
    }

    public string GetEventDetailsPageUrl(Guid calendarEventId)
    {
        return _urlHelper.RouteUrl(RouteNames.NetworkEventDetails, new { id = calendarEventId })!;
    }
}
