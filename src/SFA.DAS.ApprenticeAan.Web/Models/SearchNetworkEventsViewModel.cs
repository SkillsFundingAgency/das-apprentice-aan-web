using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class SearchNetworkEventsViewModel
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public List<CalendarEventSummary> CalendarEvents { get; set; } = new List<CalendarEventSummary>();

    public static implicit operator SearchNetworkEventsViewModel(GetCalendarEventsQueryResult result) => new()
    {
        Page = result.Page,
        PageSize = result.PageSize,
        TotalPages = result.TotalPages,
        TotalCount = result.TotalCount,
        CalendarEvents = result.CalendarEvents.ToList()
    };

}