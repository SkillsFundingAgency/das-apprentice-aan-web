using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.HtmlHelpers;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class NetworkEventsViewModel
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public bool ShowFilterOptions => StartDate.HasValue || EndDate.HasValue;
    public List<CalendarEventSummary> CalendarEvents { get; set; } = new List<CalendarEventSummary>();

    public static implicit operator NetworkEventsViewModel(GetCalendarEventsQueryResult result) => new()
    {
        Page = result.Page,
        PageSize = result.PageSize,
        TotalPages = result.TotalPages,
        TotalCount = result.TotalCount,
        CalendarEvents = result.CalendarEvents.ToList()
    };

    public string RebuildQueryRemoveField(RemoveField removeField)
    {
        var query = "?";

        if (removeField != RemoveField.StartDate)
        {
            if (StartDate != null)
                query += $"startDate={DateTimeHelper.ToUrlFormat(StartDate)}&";
        }

        if (removeField != RemoveField.EndDate)
        {
            if (EndDate != null)
            {
                query += $"endDate={DateTimeHelper.ToUrlFormat(EndDate)}&";
            }
        }

        if (query.Last() == '&')
        {
            query = query.Remove(query.Length - 1);
        }

        return query == "?" ? string.Empty : query;
    }

    public enum RemoveField
    {
        StartDate,
        EndDate
    }
}