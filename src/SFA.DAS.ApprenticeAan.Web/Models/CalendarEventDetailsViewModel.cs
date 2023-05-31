using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class CalendarEventDetailsViewModel
{
    public Guid CalendarEventId { get; set; }
    public string CalendarName { get; set; } = string.Empty;
    public string EventFormat { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Summary { get; set; } = null!;
    public string? Location { get; set; } = null!;
    public string? Postcode { get; set; } = null!;
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Distance { get; set; }
    public string? EventLink { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public bool IsAttending { get; set; }
    public string? CancelReason { get; set; } = null!;
    public IEnumerable<Attendee> Attendees { get; set; } = null!;
    public IEnumerable<EventGuest> EventGuests { get; set; } = null!;


    public static implicit operator CalendarEventDetailsViewModel(CalendarEvent source) => new()
    {
        CalendarEventId = source.CalendarEventId,
        CalendarName = source.CalendarName,
        EventFormat = source.EventFormat,
        StartDate = source.StartDate,
        EndDate = source.EndDate,
        Description = source.Description,
        Summary = source.Summary,
        Location = source.Location,
        Postcode = source.Postcode,
        Longitude = source.Longitude,
        Latitude = source.Latitude,
        Distance = source.Distance,
        EventLink = source.EventLink,
        ContactName = source.ContactName,
        ContactEmail = source.ContactEmail,
        IsAttending = source.IsActive,
        CancelReason = source.CancelReason,
        Attendees = source.Attendees,
        EventGuests = source.EventGuests,
    };

    public string GetPartialViewName()
    {
        return EventFormat.ToLower().Trim() switch
        {
            "online" => "CalendarEventOnlinePartial.cshtml",
            _ => throw new NotImplementedException($"The partial view for event format {EventFormat} is not implemented")
        };
    }
}
