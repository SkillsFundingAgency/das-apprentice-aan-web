using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.Aan.SharedUi.UrlHelpers;

namespace SFA.DAS.Aan.SharedUi.Models;
public class NetworkEventDetailsViewModel : INetworkHubLink
{
    public Guid CalendarEventId { get; set; }
    public string CalendarName { get; set; }
    public EventFormat EventFormat { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string StartTime { get; set; }
    public DateTime StartDateTime { get; set; }

    public string EndTime { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Summary { get; set; }
    public LocationDetails? LocationDetails { get; set; }
    public string? EventLink { get; set; }
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }
    public string? CancelReason { get; set; }
    public List<Attendee> Attendees { get; set; } = new List<Attendee>();
    public List<EventGuest> EventGuests { get; set; } = new List<EventGuest>();
    public bool IsSignedUp { get; set; }
    public string PartialViewName => GetPartialViewName();

    public int AttendeeCount => Attendees.Count;
    public bool IsPastEvent => StartDateTime < DateTime.UtcNow;

    public string EmailLink => MailToLinkValue.FromAddressAndSubject(ContactEmail, Title);

    public string GoogleMapsLink => LocationDetails?.Location == null ? string.Empty : $"https://www.google.com/maps/dir//{LocationDetails?.Location}+{LocationDetails?.Postcode}";

    public string EventsHubUrl { get; set; } = "#";

    public string? NetworkHubLink { get; set; }

    public string? BackLinkUrl { get; set; }
    public bool IsPreview { get; set; }

    public NetworkEventDetailsViewModel(string calendarName, DateTime start, DateTime end, string title, string description, string contactName, string contactEmail)
    {
        CalendarName = calendarName;
        StartDateTime = start;
        StartDate = start.UtcToLocalTime().ToString("dddd, d MMMM yyyy");
        EndDate = end.UtcToLocalTime().ToString("dddd, d MMMM yyyy");
        StartTime = start.UtcToLocalTime().ToString("h:mmtt").ToLower();
        EndTime = end.UtcToLocalTime().ToString("h:mmtt").ToLower();
        Title = title;
        Description = description;
        ContactName = contactName;
        ContactEmail = contactEmail;
    }

    public NetworkEventDetailsViewModel(CalendarEvent source, Guid memberId)
    {
        CalendarEventId = source.CalendarEventId;
        CalendarName = source.CalendarName;
        EventFormat = source.EventFormat;
        StartDateTime = source.StartDate;
        StartDate = StartDateTime.UtcToLocalTime().ToString("dddd, d MMMM yyyy");
        EndDate = source.EndDate.UtcToLocalTime().ToString("dddd, d MMMM yyyy");
        StartTime = StartDateTime.UtcToLocalTime().ToString("h:mmtt").ToLower();
        EndTime = source.EndDate.UtcToLocalTime().ToString("h:mmtt").ToLower();
        Title = source.Title;
        Description = source.Description;
        Summary = source.Summary;
        EventLink = source.EventLink;
        ContactName = source.ContactName;
        ContactEmail = source.ContactEmail;
        CancelReason = source.CancelReason;
        Attendees = source.Attendees;
        EventGuests = source.EventGuests;

        IsSignedUp = Attendees.Exists(a => a.MemberId == memberId);

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
