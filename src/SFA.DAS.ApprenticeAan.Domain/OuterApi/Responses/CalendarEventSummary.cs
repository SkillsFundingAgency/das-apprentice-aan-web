﻿using SFA.DAS.ApprenticeAan.Domain.Constants;

namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

public class CalendarEventSummary
{
    public Guid CalendarEventId { get; set; }
    public string CalendarName { get; set; } = null!;
    public string CalendarEventLink { get; set; } = null!;
    public EventFormat EventFormat { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Postcode { get; set; } = null!;
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Distance { get; set; }
    public bool IsAttending { get; set; }
}