using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Web.Models.CalendarEvents;

public class EventDetailsViewModel
{
    public Guid CalendarEventId { get; set; }
    public string CalendarName { get; set; } = string.Empty;
    public string EventFormat { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Summary { get; set; } = null!;
    public LocationDetails LocationDetails { get; set; }
    public double? Distance { get; set; }
    public string? EventLink { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public bool IsAttending { get; set; }
    public string? CancelReason { get; set; } = null!;
    public virtual string PartialViewName => GetPartialViewName();

    public IEnumerable<Attendee> Attendees { get; set; } = null!;
    public IEnumerable<EventGuest> EventGuests { get; set; } = null!;

    public EventDetailsViewModel(CalendarEvent source)
    {
        CalendarEventId = source.CalendarEventId;
        CalendarName = source.CalendarName;
        EventFormat = source.EventFormat;
        StartDate = source.StartDate;
        EndDate = source.EndDate;
        Description = source.Description;
        Summary = source.Summary;
        Distance = source.Distance;
        EventLink = source.EventLink;
        ContactName = source.ContactName;
        ContactEmail = source.ContactEmail;
        IsAttending = source.IsActive;
        CancelReason = source.CancelReason;
        Attendees = source.Attendees;
        EventGuests = source.EventGuests;

        if (source.EventFormat.Trim().ToLower() != "online")
        {
            LocationDetails = new LocationDetails()
            {
                Location = source.Location,
                Postcode = source.Postcode,
                Longitude = source.Longitude,
                Latitude = source.Latitude,
            };
        }
    }

    private string GetPartialViewName()
    {
        return EventFormat.Trim().ToLower() switch
        {
            "online" => "OnlineEventDetailsPartial.cshtml",
            "in person" => "InPersonEventDetailsPartial.cshtml",
            "hybrid" => "HybridEventDetailsPartial.cshtml",
            _ => throw new NotImplementedException($"Failed to find a matching partial view for event format \"{EventFormat}\""),
        };
    }
}
