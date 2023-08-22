using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.Aan.SharedUi.Models.NetworkEvents;

public class GetNetworkEventsRequest
{
    [FromQuery]
    public string? Keyword { get; set; }

    [FromQuery]
    public DateTime? FromDate { get; set; }

    [FromQuery]
    public DateTime? ToDate { get; set; }

    [FromQuery]
    public List<EventFormat> EventFormat { get; set; } = new List<EventFormat>();

    [FromQuery]
    public List<int> CalendarId { get; set; } = new List<int>();

    [FromQuery]
    public List<int> RegionId { get; set; } = new List<int>();

    [FromQuery]
    public int? Page { get; set; }

    [FromQuery]
    public int? PageSize { get; set; }
}
