using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.Models.NetworkDirectory;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;

namespace SFA.DAS.Aan.SharedUi.Services;

public static class FilterBuilder
{
    public static List<SelectedFilter> Build(GetNetworkEventsRequest request, Func<string> getNetworkEventsUrl, IEnumerable<ChecklistLookup> eventFormatLookups, IEnumerable<ChecklistLookup> eventTypeLookups, IEnumerable<ChecklistLookup> regionLookups)
    {
        var filters = new List<SelectedFilter>();
        var fullQueryParameters = BuildQueryParameters(request);

        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            var text = request.Radius == 0 ? "Across England" : $"Within {request.Radius} miles of {request.Location}";
            filters.AddFilterItemForLocation(getNetworkEventsUrl, fullQueryParameters, text, request.Location, request.Radius ?? 0);
        }

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            filters.AddFilterItems(getNetworkEventsUrl, fullQueryParameters, new[] { request.Keyword }, "Network events", "keyword", Enumerable.Empty<ChecklistLookup>());
        }

        if (request.FromDate.HasValue)
        {
            filters.AddFilterItems(getNetworkEventsUrl, fullQueryParameters, new[] { request.FromDate.Value.ToScreenString() }, "From date", "fromDate", Enumerable.Empty<ChecklistLookup>());
        }

        if (request.ToDate.HasValue)
        {
            filters.AddFilterItems(getNetworkEventsUrl, fullQueryParameters, new[] { request.ToDate.Value.ToScreenString() }, "To date", "toDate", Enumerable.Empty<ChecklistLookup>());
        }

        filters.AddFilterItems(getNetworkEventsUrl, fullQueryParameters, request.EventFormat.Select(e => e.ToString()), "Event format", "eventFormat", eventFormatLookups);

        filters.AddFilterItems(getNetworkEventsUrl, fullQueryParameters, request.CalendarId.Select(e => e.ToString()), "Event type", "calendarId", eventTypeLookups);

        filters.AddFilterItems(getNetworkEventsUrl, fullQueryParameters, request.RegionId.Select(e => e.ToString()), "Region", "regionId", regionLookups);

        return filters;
    }

    public static List<SelectedFilter> Build(NetworkDirectoryRequestModel request, Func<string> getNetworkDirectoryUrl, IEnumerable<ChecklistLookup> userTypeLookups, IEnumerable<ChecklistLookup> regionLookups)
    {
        var filters = new List<SelectedFilter>();
        var fullQueryParameters = BuildQueryParameters(request);

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            filters.AddFilterItems(getNetworkDirectoryUrl, fullQueryParameters, new[] { request.Keyword }, "Network directory", "keyword", Enumerable.Empty<ChecklistLookup>());
        }

        filters.AddFilterItems(getNetworkDirectoryUrl, fullQueryParameters, request.UserRole.Select(e => e.ToString()), "Role", "userRole", userTypeLookups);

        filters.AddFilterItems(getNetworkDirectoryUrl, fullQueryParameters, request.RegionId.Select(e => e.ToString()), "Region", "regionId", regionLookups);

        return filters;
    }

    public static string BuildFullQueryString(GetNetworkEventsRequest request, Func<string> getNetworkEventsUrl)
    {
        var fullQueryParameters = BuildQueryParameters(request);
        return BuildQueryString(getNetworkEventsUrl, fullQueryParameters, new List<string>())!;
    }

    public static string BuildFullQueryString(NetworkDirectoryRequestModel request, Func<string> getNetworkDirectoryUrl)
    {
        var fullQueryParameters = BuildQueryParameters(request);
        return BuildQueryString(getNetworkDirectoryUrl, fullQueryParameters, new List<string>())!;
    }

    public static void AddFilterItems(this ICollection<SelectedFilter> filters, Func<string> getNetworkEventsUrl, List<string> fullQueryParameters, IEnumerable<string> selectedValues, string fieldName, string parameterName, IEnumerable<ChecklistLookup> lookups)
    {
        if (!selectedValues.Any()) return;

        var filter = new SelectedFilter
        {
            FieldName = fieldName,
            FieldOrder = filters.Count + 1
        };

        int i = 0;

        foreach (var value in selectedValues)
        {
            var v = lookups.Any() ? lookups.First(l => l.Value == value).Name : value;
            filter.Filters.Add(BuildFilterItem(getNetworkEventsUrl, fullQueryParameters, [BuildQueryParameter(parameterName, value)], v, ++i));
        }

        filters.Add(filter);
    }

    private static List<string> BuildQueryParameters(GetNetworkEventsRequest request)
    {
        var queryParameters = new List<string>();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            queryParameters.Add(BuildQueryParameter("keyword", request.Keyword));
        }

        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            queryParameters.Add($"location={request.Location}");
            queryParameters.Add($"radius={request.Radius}");
        }

        if (!string.IsNullOrWhiteSpace(request.OrderBy))
        {
            queryParameters.Add($"orderBy={request.OrderBy}");
        }

        if (request.FromDate != null)
        {
            queryParameters.Add(BuildDateQueryParameter("fromDate", request.FromDate.GetValueOrDefault()));
        }

        if (request.ToDate != null)
        {
            queryParameters.Add(BuildDateQueryParameter("toDate", request.ToDate.GetValueOrDefault()));
        }

        queryParameters.AddRange(request.EventFormat.Select(eventFormat => "eventFormat=" + eventFormat));
        queryParameters.AddRange(request.CalendarId.Select(eventType => "calendarId=" + eventType));
        queryParameters.AddRange(request.RegionId.Select(region => "regionId=" + region));

        return queryParameters;
    }

    public static List<string> BuildQueryParameters(NetworkDirectoryRequestModel request)
    {
        var queryParameters = new List<string>();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            queryParameters.Add(BuildQueryParameter("keyword", request.Keyword));
        }

        if (request.UserRole != null && request.UserRole.Count != 0)
        {
            queryParameters.AddRange(request.UserRole.Select(userRole => "userRole=" + userRole));
        }

        if (request.RegionId != null && request.RegionId.Count != 0)
        {
            queryParameters.AddRange(request.RegionId.Select(region => "regionId=" + region));
        }

        if (request.Status != null && request.Status.Count != 0)
        {
            queryParameters.AddRange(request.Status.Select(s => "status=" + s));
        }

        return queryParameters;
    }

    private static EventFilterItem BuildFilterItem(Func<string> getNetworkEventsUrl, List<string> queryParameters, List<string> filtersToRemove, string filterValue, int filterFieldOrder)
        =>
        new()
        {
            ClearFilterLink = BuildQueryString(getNetworkEventsUrl, queryParameters, filtersToRemove),
            Order = filterFieldOrder,
            Value = filterValue
        };

    private static string? BuildQueryString(Func<string> getNetworkEventsUrl, List<string> queryParameters, List<string> filtersToRemove)
    {
        var queryParametersToBuild = queryParameters.Where(s => !filtersToRemove.Contains(s));

        return queryParametersToBuild.Any() ? $"{getNetworkEventsUrl()}?{string.Join('&', queryParametersToBuild)}" : getNetworkEventsUrl();
    }

    private static string BuildDateQueryParameter(string name, DateTime value) => $"{name}={value.ToApiString()}";

    private static string BuildQueryParameter(string name, string value)
    {
        return DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
            out DateTime parsedDate) ?
            $"{name}={parsedDate.ToApiString()}" :
            $"{name}={value}";
    }

    private static void AddFilterItemForLocation(this List<SelectedFilter> filters, Func<string> url, List<string> fullQueryParameters, string selectedValue, string location, int radius)
    {
        if (string.IsNullOrWhiteSpace(selectedValue)) return;

        var filter = new SelectedFilter
        {
            FieldName = "Location",
            FieldOrder = filters.Count + 1
        };

        var i = 0;

        var filtersToRemove = new List<string>()
        {
            { BuildQueryParameter("location", location)},
            { BuildQueryParameter("radius", radius.ToString())},
        };

        filter.Filters.Add(BuildFilterItem(url, fullQueryParameters, filtersToRemove, selectedValue, ++i));

        filters.Add(filter);
    }
}
