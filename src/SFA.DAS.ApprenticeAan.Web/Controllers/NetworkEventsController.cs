using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]

public class NetworkEventsController : Controller
{
    private readonly IOuterApiClient _outerApiClient;
    private readonly ApplicationConfiguration _applicationConfiguration;

    public NetworkEventsController(IOuterApiClient outerApiClient, ApplicationConfiguration applicationConfiguration)
    {
        _outerApiClient = outerApiClient;
        _applicationConfiguration = applicationConfiguration;
    }

    [HttpGet]
    [Route("network-events", Name = RouteNames.NetworkEvents)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var calendarEventsResponse = await _outerApiClient.GetCalendarEvents(User.GetAanMemberId(), cancellationToken);
        var model = (NetworkEventsViewModel)calendarEventsResponse;
        return View(model);
    }

    [HttpGet]
    [Route("network-events/{id}", Name = RouteNames.NetworkEventDetails)]
    public async Task<IActionResult> Details([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var memberId = User.GetAanMemberId();
        var eventDetailsResponse = await _outerApiClient.GetCalendarEventDetails(id, memberId, cancellationToken);

        if (eventDetailsResponse.ResponseMessage.IsSuccessStatusCode)
        {
            return View(new NetworkEventDetailsViewModel(
                eventDetailsResponse.GetContent(),
                memberId,
                _applicationConfiguration.ApplicationSettings.GoogleMapsApiKey,
                _applicationConfiguration.ApplicationSettings.GoogleMapsPrivateKey));
        }

        throw new InvalidOperationException($"An event with ID {id} was not found."); //TODO: Navigate to 404
    }
}
