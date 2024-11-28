using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using System.Drawing;

namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
public class GetCalendarEventsQueryResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<CalendarEventSummary> CalendarEvents { get; set; } = [];

    public bool IsInvalidLocation { get; set; }

    public List<Region> Regions { get; set; } = [];
    public List<Calendar> Calendars { get; set; } = [];
}
