using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.HtmlHelpers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;


namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-events")]
public class NetworkEventsController : Controller
{
    private readonly IOuterApiClient _outerApiClient;
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly IEventSearchQueryStringBuilder _builder;

    public NetworkEventsController(IOuterApiClient outerApiClient, IEventSearchQueryStringBuilder builder, ApplicationConfiguration applicationConfiguration)
    {
        _outerApiClient = outerApiClient;
        _builder = builder;
        _applicationConfiguration = applicationConfiguration;
    }

    [HttpGet]
    [Route("", Name = RouteNames.NetworkEvents)]
    public async Task<IActionResult> Index(GetNetworkEventsRequest request, CancellationToken cancellationToken)
    {
        var fromDateFormatted = DateTimeHelper.ToUrlFormat(request.FromDate)!;
        var toDateFormatted = DateTimeHelper.ToUrlFormat(request.ToDate)!;
        var calendarEventsResponse = await _outerApiClient.GetCalendarEvents(User.GetAanMemberId(), fromDateFormatted,
            toDateFormatted, request.EventFormat, cancellationToken);
        var model = (NetworkEventsViewModel)calendarEventsResponse;


        model.EventFilters.FromDate = request.FromDate;
        model.EventFilters.ToDate = request.ToDate;
        model.EventFilters.EventFormats = request.EventFormat;

        model.SearchFilters = _builder.BuildEventSearchFilters(model.EventFilters, Url);


        return View(model);
    }

    [HttpGet]
    [Route("{id}", Name = RouteNames.NetworkEventDetails)]
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
