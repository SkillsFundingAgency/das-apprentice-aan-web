namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-events")]
public class NetworkEventsController : Controller
{
    private readonly IOuterApiClient _outerApiClient;
    private readonly string _googleMapsApiKey;
    private readonly string _googleMapsPrivateKey;

    public NetworkEventsController(IOuterApiClient outerApiClient, ApplicationConfiguration config)
    {
        _outerApiClient = outerApiClient;
        _googleMapsApiKey = config.ApplicationSettings.GoogleMapsApiKey;
        _googleMapsPrivateKey = config.ApplicationSettings.GoogleMapsPrivateKey;
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
        var eventDetailsResponse = await _outerApiClient.GetCalendarEventDetails(id, memberId, cancellationToken);

        if (eventDetailsResponse.ResponseMessage.IsSuccessStatusCode)
        {
            return View(new NetworkEventDetailsViewModel(eventDetailsResponse.GetContent(), memberId));
        }

        throw new InvalidOperationException($"An event with ID {id} was not found."); //TODO: Navigate to 404
    }
}
