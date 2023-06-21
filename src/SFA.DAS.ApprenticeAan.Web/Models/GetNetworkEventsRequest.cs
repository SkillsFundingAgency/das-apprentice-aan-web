using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class GetNetworkEventsRequest
{
    [FromQuery]
    public DateTime? FromDate { get; set; }

    [FromQuery]
    public DateTime? ToDate { get; set; }

    [FromQuery] public List<EventFormat> EventFormat { get; set; } = new List<EventFormat>();

    [FromQuery] public List<int> CalendarId { get; set; } = new List<int>();
}