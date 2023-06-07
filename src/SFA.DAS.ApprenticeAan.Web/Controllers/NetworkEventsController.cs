using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-events")]
public class NetworkEventsController : Controller
{
    private readonly IOuterApiClient _outerApiClient;

    public NetworkEventsController(IOuterApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }

    [HttpGet]
    [Route("", Name = RouteNames.NetworkEvents)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var calendarEventsResponse = await _outerApiClient.GetCalendarEvents(User.GetAanMemberId(), cancellationToken);
        var model = (NetworkEventsViewModel)calendarEventsResponse;
        return View(model);
    }

    [HttpGet]
    [Route("{id}", Name = RouteNames.NetworkEventDetails)]
    public async Task<IActionResult> Details([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var memberId = User.GetAanMemberId();
        var eventDetails = await _outerApiClient.GetCalendarEventDetails(id, memberId, cancellationToken);
        return eventDetails.CalendarEventId != Guid.Empty
            ? View(new NetworkEventDetailsViewModel(eventDetails, memberId))
            : RedirectToRoute(RouteNames.NetworkEvents);
    }
}
