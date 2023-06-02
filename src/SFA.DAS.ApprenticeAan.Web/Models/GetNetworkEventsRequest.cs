using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class GetNetworkEventsRequest
{
    [FromQuery]
    public DateTime? StartDate { get; set; }

    [FromQuery]
    public DateTime? EndDate { get; set; }

    [FromQuery]
    public List<EventFormat>? EventFormats { get; set; }
}