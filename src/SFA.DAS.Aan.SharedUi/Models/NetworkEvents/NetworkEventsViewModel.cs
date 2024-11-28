using Microsoft.AspNetCore.Mvc.Rendering;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;

namespace SFA.DAS.Aan.SharedUi.Models.NetworkEvents;

public class NetworkEventsViewModel : INetworkHubLink
{
    public PaginationViewModel PaginationViewModel { get; set; } = null!;

    public int TotalCount { get; set; }

    public List<CalendarEventViewModel> CalendarEvents { get; set; } = new List<CalendarEventViewModel>();

    public EventFilterChoices FilterChoices { get; set; } = new EventFilterChoices();

    public SelectedFiltersModel SelectedFiltersModel { get; set; } = new SelectedFiltersModel();
    public string? NetworkHubLink { get; set; }

    public string OrderBy { get; set; }

    public List<SelectListItem> OrderByOptions => new List<SelectListItem>
        { new("Soonest", "soonest"), new("Closest", "closest") };

    public bool ShowSortOptions => !string.IsNullOrWhiteSpace(FilterChoices.Location) && CalendarEvents.Any();

    public bool IsInvalidLocation { get; set; }

    public string SearchedLocation { get; set; } = string.Empty;

    public bool ShowCalendarEvents => CalendarEvents.Any();
}

public class CalendarEventViewModel
{
    public Guid CalendarEventId { get; set; }
    public string CalendarName { get; set; } = null!;
    public EventFormat EventFormat { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Title { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Postcode { get; set; } = null!;
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Distance { get; set; }
    public bool IsAttending { get; set; }
    public string? CalendarEventLink { get; set; }

    public static implicit operator CalendarEventViewModel(CalendarEventSummary source)
        => new()
        {
            CalendarEventId = source.CalendarEventId,
            CalendarName = source.CalendarName,
            EventFormat = source.EventFormat,
            Start = source.Start,
            End = source.End,
            Title = source.Title,
            Summary = source.Summary,
            Location = source.Location,
            Postcode = source.Postcode,
            Longitude = source.Longitude,
            Latitude = source.Latitude,
            Distance = source.Distance,
            IsAttending = source.IsAttending
        };
}