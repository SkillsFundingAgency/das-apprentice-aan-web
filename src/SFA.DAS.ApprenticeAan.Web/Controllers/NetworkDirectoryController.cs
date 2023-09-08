using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;
using SFA.DAS.Aan.SharedUi.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkDirectory;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.ApprenticeAan.Web.Controllers;

[Authorize]
[Route("network-directory")]
public class NetworkDirectoryController : Controller
{
    private readonly IOuterApiClient _outerApiClient;

    public NetworkDirectoryController(IOuterApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }

    [HttpGet]
    [Route("", Name = SharedRouteNames.NetworkDirectory)]
    public async Task<IActionResult> Index(GetNetworkDirectoryRequest request, CancellationToken cancellationToken)
    {
        var networkDirectoryTask = _outerApiClient.GetMembers(QueryStringParameterBuilder.BuildQueryStringParameters(request), cancellationToken);
        var regionTask = _outerApiClient.GetRegions();

        List<Task> tasks = new() { networkDirectoryTask, regionTask };

        await Task.WhenAll(tasks);
        var regions = regionTask.Result.Regions;
        regions.Add(new Region() { Area = "Multi-regional", Id = 0, Ordering = regions.Count + 1 });

        var model = InitialiseViewModel(networkDirectoryTask.Result);
        var filterUrl = FilterBuilder.BuildFullQueryString(request, () => Url.RouteUrl(SharedRouteNames.NetworkDirectory)!);
        model.PaginationViewModel = SetupPagination(networkDirectoryTask.Result, filterUrl);
        var filterChoices = PopulateFilterChoices(request, regions);
        model.FilterChoices = filterChoices;
        model.SelectedFilters = FilterBuilder.Build(request, () => Url.RouteUrl(SharedRouteNames.NetworkDirectory)!, filterChoices.RoleChecklistDetails.Lookups, filterChoices.RegionChecklistDetails.Lookups);
        model.ClearSelectedFiltersLink = Url.RouteUrl(SharedRouteNames.NetworkDirectory)!;
        return View(model);
    }

    private NetworkDirectoryViewModel InitialiseViewModel(GetNetworkDirectoryQueryResult result)
    {
        var model = new NetworkDirectoryViewModel
        {
            TotalCount = result.TotalCount,

        };

        foreach (var networkDirectory in result.Members)
        {
            model.Members.Add(networkDirectory);
        }
        return model;
    }

    private static PaginationViewModel SetupPagination(GetNetworkDirectoryQueryResult result, string filterUrl)
    {
        var pagination = new PaginationViewModel(result.Page, result.PageSize, result.TotalPages, filterUrl);

        return pagination;

    }
    private static DirectoryFilterChoices PopulateFilterChoices(GetNetworkDirectoryRequest request, List<Region> regions)
        => new DirectoryFilterChoices
        {
            Keyword = request.Keyword?.Trim(),
            RoleChecklistDetails = new ChecklistDetails
            {
                Title = "Role",
                QueryStringParameterName = "userType",
                Lookups = new ChecklistLookup[]
                {
                    new(Role.Apprentice.GetDescription()!, Role.Apprentice.ToString(), request.UserType.Exists(x => x == Role.Apprentice)),
                    new(Role.Employer.GetDescription()!, Role.Employer.ToString(), request.UserType.Exists(x => x == Role.Employer)),
                    new(Role.IsRegionalChair.GetDescription()!, Role.IsRegionalChair.ToString(), request.UserType.Exists(x => x == Role.IsRegionalChair))
                }
            },
            RegionChecklistDetails = new ChecklistDetails
            {
                Title = "Region",
                QueryStringParameterName = "regionId",
                Lookups = regions.OrderBy(x => x.Ordering).Select(region => new ChecklistLookup(region.Area, region.Id.ToString(), request.RegionId.Exists(x => x == region.Id))).ToList()
            }
        };
}
