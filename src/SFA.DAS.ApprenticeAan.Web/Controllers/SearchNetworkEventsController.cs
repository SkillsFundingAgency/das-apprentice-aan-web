

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;


namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("search-network-events", Name = RouteNames.SearchNetworkEvents)]
public class SearchNetworkEventsController : Controller
{
    private readonly IOuterApiClient _outerApiClient;

    public SearchNetworkEventsController(IOuterApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var calendarEventsResponse = await _outerApiClient.GetCalendarEvents(User.GetAanMemberId().ToString(), cancellationToken);
        var model = (SearchNetworkEventsViewModel)calendarEventsResponse;
        return View(model);
    }
}