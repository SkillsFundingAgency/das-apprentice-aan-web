using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class NetworkEventsViewModel
{
    public PaginationModel Pagination { get; set; } = new PaginationModel();

    public int TotalCount { get; set; }

    public List<CalendarEventSummary> CalendarEvents { get; set; } = new List<CalendarEventSummary>();

    public EventFilterChoices FilterChoices { get; set; } = new EventFilterChoices();

    public List<SelectedFilter> SelectedFilters { get; set; } = new List<SelectedFilter>();


    public bool ShowFilterOptions => SelectedFilters.Any();


    public static implicit operator NetworkEventsViewModel(GetCalendarEventsQueryResult result) => new()
    {
        Pagination = new PaginationModel
        {
            Page = result.Page,
            PageSize = result.PageSize,
            TotalPages = result.TotalPages
        },
        TotalCount = result.TotalCount,
        CalendarEvents = result.CalendarEvents.ToList()
    };
}