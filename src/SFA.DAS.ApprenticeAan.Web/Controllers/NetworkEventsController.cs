using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Extensions;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.Models;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-events")]
public class NetworkEventsController : Controller
{
    public const string SignUpConfirmationViewPath = "~/Views/NetworkEvents/SignUpConfirmation.cshtml";
    public const string CancellationConfirmationViewPath = "~/Views/NetworkEvents/CancellationConfirmation.cshtml";

    private readonly IOuterApiClient _outerApiClient;
    private readonly ApplicationConfiguration _applicationConfiguration;
    private readonly IEventSearchQueryStringBuilder _builder;

    public NetworkEventsController(IOuterApiClient outerApiClient, IEventSearchQueryStringBuilder builder, ApplicationConfiguration applicationConfiguration)
    {
        _outerApiClient = outerApiClient;
        _applicationConfiguration = applicationConfiguration;
        _builder = builder;
    }

    [HttpGet]
    [Route("", Name = RouteNames.NetworkEvents)]
    public async Task<IActionResult> Index(GetNetworkEventsRequest request, CancellationToken cancellationToken)
    {
        var fromDateFormatted = request.FromDate?.ToApiString()!;
        var toDateFormatted = request.ToDate?.ToApiString()!;
        var calendarEventsResponse = await _outerApiClient.GetCalendarEvents(User.GetAanMemberId(), fromDateFormatted,
            toDateFormatted, request.EventFormat, request.CalendarId, request.RegionId, cancellationToken);
        var model = (NetworkEventsViewModel)calendarEventsResponse;

        foreach (var calendarEvent in model.CalendarEvents)
        {
            calendarEvent.CalendarEventLink = Url.RouteUrl(RouteNames.NetworkEventDetails, new { id = calendarEvent.CalendarEventId })!;
        }

        var calendars = await _outerApiClient.GetCalendars();
        var regionsResult = await _outerApiClient.GetRegions();
        model.Regions = regionsResult.Regions;

        model.FilterChoices = new EventFilterChoices
        {
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            EventFormats = request.EventFormat,
            CalendarIds = request.CalendarId,
            RegionIds = request.RegionId,
            EventFormatsLookup = new List<ChecklistLookup>
            {
                new(EventFormat.InPerson.GetDescription()!, EventFormat.InPerson.ToString()),
                new(EventFormat.Online.GetDescription()!, EventFormat.Online.ToString()),
                new(EventFormat.Hybrid.GetDescription()!, EventFormat.Hybrid.ToString())
            },
            EventTypesLookup = model.Calendars.OrderBy(x => x.Ordering)
                .Select(cal => new ChecklistLookup(cal.CalendarName, cal.Id.ToString())).ToList(),
            RegionsLookup = model.Regions.OrderBy(x => x.Ordering)
                .Select(region => new ChecklistLookup(region.Area, region.Id.ToString())).ToList()
        };

        // MFCMFC model.FilterChoices.EventTypesLookup = calendars.OrderBy(x => x.Ordering).Select(cal => new ChecklistLookup(cal.CalendarName, cal.Id.ToString())).ToList();
        model.SelectedFilters = _builder.BuildEventSearchFilters(model.FilterChoices, Url);

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

    [HttpGet]
    [Route("signup-confirmation", Name = RouteNames.AttendanceConfirmations.SignUpConfirmation)]
    public IActionResult SignUpConfirmation()
    {
        return View(SignUpConfirmationViewPath);
    }

    [HttpGet]
    [Route("cancellation-confirmation", Name = RouteNames.AttendanceConfirmations.CancellationConfirmation)]
    public IActionResult CancellationConfirmation()
    {
        return View(CancellationConfirmationViewPath);
    }

    [HttpPost]
    public async Task<IActionResult> SetAttendanceStatus(Guid calendarEventId, bool newStatus)
    {
        var memberId = User.GetAanMemberId();
        await _outerApiClient.PutAttendance(calendarEventId, memberId, new SetAttendanceStatusRequest(newStatus), new CancellationToken());
        return newStatus
            ? RedirectToAction("SignUpConfirmation")
            : RedirectToAction("CancellationConfirmation");
    }
}
