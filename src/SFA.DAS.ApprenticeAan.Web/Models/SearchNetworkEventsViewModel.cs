using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class SearchNetworkEventsViewModel
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public List<CalendarEventSummary> CalendarEvents { get; set; } = new List<CalendarEventSummary>();
}