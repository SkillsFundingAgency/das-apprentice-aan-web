using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class GetNetworkEventsRequest
{
    [FromQuery]
    public DateTime? FromDate { get; set; }

    [FromQuery]
    public DateTime? ToDate { get; set; }
}