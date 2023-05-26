

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-events", Name = RouteNames.NetworkEvents)]
public class NetworkEventsController : Controller
{
    private readonly IOuterApiClient _outerApiClient;

    public NetworkEventsController(IOuterApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }
    [HttpGet]
    public async Task<IActionResult> Index(GetNetworkEventsRequest request, CancellationToken cancellationToken)
    {

        var calendarEventsResponse = await _outerApiClient.GetCalendarEvents(User.GetAanMemberId(), request.StartDate?.ToString("yyyy-MM-dd"), request.EndDate?.ToString("yyyy-MM-dd"), cancellationToken);
        var model = (NetworkEventsViewModel)calendarEventsResponse;
        model.StartDate = request.StartDate;
        model.EndDate = request.EndDate;
        return View(model);
    }
}