using SFA.DAS.ApprenticeAan.Domain.Models;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class NetworkEventsViewModel
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public EventFilterChoices EventFilterChoices { get; set; } = new EventFilterChoices();

    public List<Calendar> Calendars { get; set; } = new List<Calendar>();
    public List<ChecklistLookup> EventFormats { get; set; } = new List<ChecklistLookup>();
    public bool ShowFilterOptions => SearchFilters.Any();
    public List<CalendarEventSummary> CalendarEvents { get; set; } = new List<CalendarEventSummary>();

    public List<SelectedFilter> SearchFilters { get; set; } = new List<SelectedFilter>();

    public static implicit operator NetworkEventsViewModel(GetCalendarEventsQueryResult result) => new()
    {
        Page = result.Page,
        PageSize = result.PageSize,
        TotalPages = result.TotalPages,
        TotalCount = result.TotalCount,
        CalendarEvents = result.CalendarEvents.ToList()
    };

}
