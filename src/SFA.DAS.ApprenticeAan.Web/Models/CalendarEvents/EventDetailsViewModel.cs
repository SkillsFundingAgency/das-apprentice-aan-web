using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Web.Models.CalendarEvents;

public class EventDetailsViewModel
{
    public Guid CalendarEventId { get; set; }
    public string CalendarName { get; set; } = string.Empty;
    public EventFormat EventFormat { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Summary { get; set; } = null!;
    public LocationDetails LocationDetails { get; set; }
    public double? Distance { get; set; }
    public string? EventLink { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
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
        EventLink = source.EventLink;
        ContactName = source.ContactName;
        ContactEmail = source.ContactEmail;
        CancelReason = source.CancelReason;
        Attendees = source.Attendees;
        EventGuests = source.EventGuests;

        if (EventFormat != EventFormat.Online)
        {
            LocationDetails = new LocationDetails()
            {
                Location = source.Location,
                Postcode = source.Postcode,
                Longitude = source.Longitude,
                Latitude = source.Latitude,
                Distance = source.Distance,
            };
        }
    }

    private string GetPartialViewName()
    {
        return EventFormat switch
        {
            EventFormat.Online => "OnlineEventDetailsPartial.cshtml",
            EventFormat.InPerson => "InPersonEventDetailsPartial.cshtml",
            EventFormat.Hybrid => "HybridEventDetailsPartial.cshtml",
            _ => throw new NotImplementedException($"Failed to find a matching partial view for event format \"{EventFormat}\""),
        };
    }
}
