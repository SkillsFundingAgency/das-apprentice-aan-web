namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class NetworkEventDetailsViewModel
{
    public Guid CalendarEventId { get; init; }
    public string CalendarName { get; init; }
    public EventFormat EventFormat { get; init; }
    public string StartDate { get; init; }
    public string EndDate { get; init; }
    public string StartTime { get; init; }
    public string EndTime { get; init; }
    public string Description { get; init; }
    public string? Summary { get; init; }
    public LocationDetails? LocationDetails { get; init; }
    public string? EventLink { get; init; }
    public string ContactName { get; init; }
    public string ContactEmail { get; init; }
    public string? CancelReason { get; init; }
    public string PartialViewName => GetPartialViewName();
    public IReadOnlyList<Attendee> Attendees { get; }
    public int AttendeeCount => Attendees.Count;
    public IReadOnlyList<EventGuest> EventGuests { get; }
    public bool IsSignedUp { get; init; }
    public string EmailLink => MailtoLinkValue.FromAddressAndSubject(ContactEmail, Description);

    public NetworkEventDetailsViewModel(CalendarEvent source, Guid memberId, string googleMapsApiKey)
    {
        CalendarEventId = source.CalendarEventId;
        CalendarName = source.CalendarName;
        EventFormat = source.EventFormat;
        StartDate = source.StartDate.ToString("dddd, d MMMM yyyy");
        EndDate = source.EndDate.ToString("dddd, d MMMM yyyy");
        StartTime = source.StartDate.ToString("h:mm tt");
        EndTime = source.EndDate.ToString("h:mm tt");
        Description = source.Description;
        Summary = source.Summary;
        EventLink = source.EventLink;
        ContactName = source.ContactName;
        ContactEmail = source.ContactEmail;
        CancelReason = source.CancelReason;
        Attendees = source.Attendees;
        EventGuests = source.EventGuests;

        IsSignedUp = Attendees.Any(a => a.MemberId == memberId);

        GoogleMapsApiKey = googleMapsApiKey;

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
            EventFormat.Online => "_OnlineEventPartial.cshtml",
            EventFormat.Hybrid => "_HybridEventPartial.cshtml",
            _ => throw new NotImplementedException($"Failed to find a matching partial view for event format \"{EventFormat}\""),
        };
    }
}
