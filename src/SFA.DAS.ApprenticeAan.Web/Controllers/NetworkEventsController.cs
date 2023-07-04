using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Extensions;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-events")]
public class NetworkEventsController : Controller
{
    public const string SignUpConfirmationViewPath = "~/Views/NetworkEvents/SignUpConfirmation.cshtml";
    public const string CancellationConfirmationViewPath = "~/Views/NetworkEvents/CancellationConfirmation.cshtml";

    private readonly IOuterApiClient _outerApiClient;
    private readonly ApplicationConfiguration _applicationConfiguration;

    public NetworkEventsController(IOuterApiClient outerApiClient, ApplicationConfiguration applicationConfiguration)
    {
        _outerApiClient = outerApiClient;
        _applicationConfiguration = applicationConfiguration;
    }

    [HttpGet]
    [Route("", Name = RouteNames.NetworkEvents)]
    public async Task<IActionResult> Index(GetNetworkEventsRequest request, CancellationToken cancellationToken)
    {
        var fromDateFormatted = request.FromDate?.ToApiString()!;
        var toDateFormatted = request.ToDate?.ToApiString()!;

        var calendarEventsTask = _outerApiClient.GetCalendarEvents(User.GetAanMemberId(), fromDateFormatted, toDateFormatted, request.EventFormat, request.CalendarId, request.RegionId, cancellationToken);
        var calendarTask = _outerApiClient.GetCalendars();
        var regionTask = _outerApiClient.GetRegions();

        List<Task> tasks = new() { calendarEventsTask, calendarTask, regionTask };

        await Task.WhenAll(tasks);

        var calendars = calendarTask.Result;
        var regions = regionTask.Result.Regions;

        var model = InitialiseViewModel(calendarEventsTask.Result);
        var filterChoices = PopulateFilterChoices(request, calendars, regions);
        model.FilterChoices = filterChoices;
        model.SelectedFilters = FilterBuilder.Build(request, Url, filterChoices.EventFormatChecklistDetails.Lookups, filterChoices.EventTypeChecklistDetails.Lookups, filterChoices.RegionChecklistDetails.Lookups);

        return View(model);
    }

    private NetworkEventsViewModel InitialiseViewModel(GetCalendarEventsQueryResult result)
    {
        NetworkEventsViewModel model = new()
        {
            Pagination = new PaginationModel
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages
            },
            TotalCount = result.TotalCount
        };

        foreach (var calendarEvent in result.CalendarEvents)
        {
            CalendarEventViewModel vm = calendarEvent;
            vm.CalendarEventLink = Url.RouteUrl(RouteNames.NetworkEventDetails, new { id = calendarEvent.CalendarEventId })!;
            model.CalendarEvents.Add(vm);
        }
        return model;
    }

    private static EventFilterChoices PopulateFilterChoices(GetNetworkEventsRequest request, List<Calendar> calendars, List<Region> regions)
        => new EventFilterChoices
        {
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            EventFormatChecklistDetails = new ChecklistDetails
            {
                Title = "Event formats",
                QueryStringParameterName = "eventFormat",
                Lookups = new ChecklistLookup[]
                {
                    new(EventFormat.InPerson.GetDescription()!, EventFormat.InPerson.ToString(), request.EventFormat.Exists(x => x == EventFormat.InPerson)),
                    new(EventFormat.Online.GetDescription()!, EventFormat.Online.ToString(), request.EventFormat.Exists(x => x == EventFormat.Online)),
                    new(EventFormat.Hybrid.GetDescription()!, EventFormat.Hybrid.ToString(), request.EventFormat.Exists(x => x == EventFormat.Hybrid))
                }
            },
            EventTypeChecklistDetails = new ChecklistDetails
            {
                Title = "Event types",
                QueryStringParameterName = "calendarId",
                Lookups = calendars.OrderBy(x => x.Ordering).Select(cal => new ChecklistLookup(cal.CalendarName, cal.Id.ToString(), request.CalendarId.Exists(x => x == cal.Id))).ToList(),
            },
            RegionChecklistDetails = new ChecklistDetails
            {
                Title = "Regions",
                QueryStringParameterName = "regionId",
                Lookups = regions.OrderBy(x => x.Ordering).Select(region => new ChecklistLookup(region.Area, region.Id.ToString(), request.RegionId.Exists(x => x == region.Id))).ToList()
            }
        };

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
