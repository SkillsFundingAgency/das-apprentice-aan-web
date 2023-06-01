using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class GetNetworkEventsRequest
{
    [FromQuery]
    public DateTime? StartDate { get; set; }

    [FromQuery]
    public DateTime? EndDate { get; set; }
}