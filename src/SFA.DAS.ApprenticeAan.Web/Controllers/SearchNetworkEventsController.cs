

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("search-network-events", Name = RouteNames.SearchNetworkEvents)]
public class SearchNetworkEventsController : Controller
{
    private readonly IOuterApiClient _outerApiClient;
    private readonly AuthenticatedUser _authenticatedUser;

    public SearchNetworkEventsController(IOuterApiClient outerApiClient, AuthenticatedUser authenticatedUser)
    {
        _outerApiClient = outerApiClient;
        _authenticatedUser = authenticatedUser;
    }
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var apprentice = await _outerApiClient.GetApprentice(_authenticatedUser.ApprenticeId);
        var calendarEventsResponse = await _outerApiClient.GetCalendarEvents(apprentice.GetContent()!.MemberId.ToString(), cancellationToken);


        var model = new SearchNetworkEventsViewModel();


        return View(model);
    }
}