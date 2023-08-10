using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.Aan.SharedUi.UrlHelpers;

namespace SFA.DAS.Aan.SharedUi.Models;
public class NetworkEventDetailsViewModel
{
    public Guid CalendarEventId { get; init; }
    public string CalendarName { get; init; }
    public EventFormat EventFormat { get; init; }
    public string StartDate { get; init; }
    public string EndDate { get; init; }
    public string StartTime { get; init; }
    public DateTime StartDateTime { get; init; }

    public string EndTime { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string? Summary { get; init; }
    public LocationDetails? LocationDetails { get; init; }
    public string? EventLink { get; init; }
    public string ContactName { get; init; }
    public string ContactEmail { get; init; }
    public string? CancelReason { get; init; }
    public IReadOnlyList<Attendee> Attendees { get; }
    public IReadOnlyList<EventGuest> EventGuests { get; }
    public bool IsSignedUp { get; init; }
    public string PartialViewName => GetPartialViewName();

    public int AttendeeCount => Attendees.Count;
    public bool IsPastEvent => StartDateTime < DateTime.UtcNow;

    public string EmailLink => MailToLinkValue.FromAddressAndSubject(ContactEmail, Title);

    public string GoogleMapsLink => LocationDetails?.Location == null ? string.Empty : $"https://www.google.com/maps/dir//{LocationDetails?.Location}+{LocationDetails?.Postcode}";

    public string EventsHubUrl { get; set; } = "#";

    public NetworkEventDetailsViewModel(CalendarEvent source, Guid memberId)
    {
        CalendarEventId = source.CalendarEventId;
        CalendarName = source.CalendarName;
        EventFormat = source.EventFormat;
        StartDateTime = source.StartDate;
        StartDate = StartDateTime.UtcToLocalTime().ToString("dddd, d MMMM yyyy");
        EndDate = source.EndDate.UtcToLocalTime().ToString("dddd, d MMMM yyyy");
        StartTime = StartDateTime.UtcToLocalTime().ToString("h:mm tt");
        EndTime = source.EndDate.UtcToLocalTime().ToString("h:mm tt");
        Title = source.Title;
        Description = source.Description;
        Summary = source.Summary;
        EventLink = source.EventLink;
        ContactName = source.ContactName;
        ContactEmail = source.ContactEmail;
        CancelReason = source.CancelReason;
        Attendees = source.Attendees;
        EventGuests = source.EventGuests;

        IsSignedUp = Attendees.Any(a => a.MemberId == memberId);

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
            EventFormat.InPerson => "_InPersonEventPartial.cshtml",
            EventFormat.Hybrid => "_HybridEventPartial.cshtml",
            _ => throw new NotImplementedException($"Failed to find a matching partial view for event format \"{EventFormat}\""),
        };
    }
}
