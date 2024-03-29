﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;
using SFA.DAS.Aan.SharedUi.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-events")]
public class NetworkEventsController : Controller
{
    private readonly IOuterApiClient _outerApiClient;
    private readonly ISessionService _sessionService;

    public NetworkEventsController(IOuterApiClient outerApiClient, ISessionService sessionService)
    {
        _outerApiClient = outerApiClient;
        _sessionService = sessionService;
    }

    [HttpGet]
    [Route("", Name = SharedRouteNames.NetworkEvents)]
    public async Task<IActionResult> Index(GetNetworkEventsRequest request, CancellationToken cancellationToken)
    {
        var calendarEventsTask = _outerApiClient.GetCalendarEvents(_sessionService.GetMemberId(), QueryStringParameterBuilder.BuildQueryStringParameters(request), cancellationToken);
        var calendarTask = _outerApiClient.GetCalendars();
        var regionTask = _outerApiClient.GetRegions();

        List<Task> tasks = [calendarEventsTask, calendarTask, regionTask];

        await Task.WhenAll(tasks);

        var calendars = calendarTask.Result;
        var regions = regionTask.Result.Regions;
        regions.Add(new Region { Area = "National", Id = 0, Ordering = regions.Select(region => region.Ordering).Max() + 1 });

        var model = InitialiseViewModel(calendarEventsTask.Result);
        var filterUrl = FilterBuilder.BuildFullQueryString(request, () => Url.RouteUrl(SharedRouteNames.NetworkEvents)!);
        model.PaginationViewModel = SetupPagination(calendarEventsTask.Result, filterUrl);
        var filterChoices = PopulateFilterChoices(request, calendars, regions);
        model.FilterChoices = filterChoices;
        model.SelectedFiltersModel.SelectedFilters = FilterBuilder.Build(request, () => Url.RouteUrl(SharedRouteNames.NetworkEvents)!, filterChoices.EventFormatChecklistDetails.Lookups, filterChoices.EventTypeChecklistDetails.Lookups, filterChoices.RegionChecklistDetails.Lookups);
        model.SelectedFiltersModel.ClearSelectedFiltersLink = Url.RouteUrl(SharedRouteNames.NetworkEvents)!;
        return View(model);
    }

    private NetworkEventsViewModel InitialiseViewModel(GetCalendarEventsQueryResult result)
    {
        var model = new NetworkEventsViewModel
        {
            TotalCount = result.TotalCount
        };

        foreach (var calendarEvent in result.CalendarEvents)
        {
            CalendarEventViewModel vm = calendarEvent;
            vm.CalendarEventLink = Url.RouteUrl(SharedRouteNames.NetworkEventDetails, new { id = calendarEvent.CalendarEventId })!;
            model.CalendarEvents.Add(vm);
        }
        return model;
    }

    private static PaginationViewModel SetupPagination(GetCalendarEventsQueryResult result, string filterUrl)
    {
        var pagination = new PaginationViewModel(result.Page, result.PageSize, result.TotalPages, filterUrl);

        return pagination;

    }
    private static EventFilterChoices PopulateFilterChoices(GetNetworkEventsRequest request, List<Calendar> calendars, List<Region> regions)
        => new()
        {
            Keyword = request.Keyword?.Trim(),
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            EventFormatChecklistDetails = new ChecklistDetails
            {
                Title = "Event formats",
                QueryStringParameterName = "eventFormat",
                Lookups = new ChecklistLookup[]
                {
                    new(EventFormat.InPerson.GetDescription(), EventFormat.InPerson.ToString(), request.EventFormat.Exists(x => x == EventFormat.InPerson)),
                    new(EventFormat.Online.GetDescription(), EventFormat.Online.ToString(), request.EventFormat.Exists(x => x == EventFormat.Online)),
                    new(EventFormat.Hybrid.GetDescription(), EventFormat.Hybrid.ToString(), request.EventFormat.Exists(x => x == EventFormat.Hybrid))
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
}
