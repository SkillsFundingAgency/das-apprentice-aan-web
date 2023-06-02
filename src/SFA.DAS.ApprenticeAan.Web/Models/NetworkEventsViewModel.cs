using SFA.DAS.ApprenticeAan.Domain.Constants;
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
    public List<EventFormat>? EventFormats { get; set; }


    public bool ShowFilterOptions => StartDate.HasValue || EndDate.HasValue || (EventFormats != null && EventFormats.Any());
    public List<CalendarEventSummary> CalendarEvents { get; set; } = new List<CalendarEventSummary>();

    public static implicit operator NetworkEventsViewModel(GetCalendarEventsQueryResult result) => new()
    {
        Page = result.Page,
        PageSize = result.PageSize,
        TotalPages = result.TotalPages,
        TotalCount = result.TotalCount,
        CalendarEvents = result.CalendarEvents.ToList()
    };

    public string BuildQueryString(FilterFields removeFilter)
    {
        var query = "?";

        if (removeFilter != FilterFields.StartDate && StartDate != null)
        {
            query += $"startDate={DateTimeHelper.ToUrlFormat(StartDate)}&";
        }

        if (removeFilter != FilterFields.EndDate && EndDate != null)
        {
            query += $"endDate={DateTimeHelper.ToUrlFormat(EndDate)}&";
        }

        if (EventFormats != null)
        {
            foreach (var eventFormat in EventFormats)
            {
                switch (eventFormat)
                {
                    case EventFormat.Online when removeFilter == FilterFields.EventFormatOnline:
                    case EventFormat.Hybrid when removeFilter == FilterFields.EventFormatHybrid:
                    case EventFormat.InPerson when removeFilter == FilterFields.EventFormatInPerson:
                        continue;
                    default:
                        query += $"eventFormats={eventFormat}&";
                        break;
                }
            }
        }

        if (query.Last() == '&')
        {
            query = query.Remove(query.Length - 1);
        }


        return query == "?" ? string.Empty : query;
    }

    public enum FilterFields
    {
        StartDate,
        EndDate,
        EventFormatOnline,
        EventFormatHybrid,
        EventFormatInPerson
    }
}