using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class NetworkEventDetailsViewModel
{
    public Guid CalendarEventId { get; init; }
    public string CalendarName { get; init; }
    public EventFormat EventFormat { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string Description { get; init; }
    public string? Summary { get; init; } = null!;
    public LocationDetails LocationDetails { get; init; }
    public string? EventLink { get; init; }
    public string ContactName { get; init; }
    public string ContactEmail { get; init; }
    public string? CancelReason { get; init; }
    public virtual string PartialViewName => GetPartialViewName();
    public IReadOnlyList<Attendee> Attendees { get; }
    public IReadOnlyList<EventGuest> EventGuests { get; }
    public bool IsSignedUp { get; init; }

    public NetworkEventDetailsViewModel(CalendarEvent source, Guid memberId)
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
            _ => throw new NotImplementedException($"Failed to find a matching partial view for event format \"{EventFormat}\""),
        };
    }
}
