using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.CalendarEvents;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("calendar-events", Name = RouteNames.CalendarEventDetails)]
public class CalendarEventsController : Controller
{
    private readonly IOuterApiClient _outerApiClient;

    public CalendarEventsController(IOuterApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }

    [Route("{id}")]
    public async Task<IActionResult> Index([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var memberId = User.GetAanMemberId();
        var eventDetails = await _outerApiClient.GetEventDetails(id, memberId, cancellationToken);
        return View(new EventDetailsViewModel(eventDetails));
    }
}
