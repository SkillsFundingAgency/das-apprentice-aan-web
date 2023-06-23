using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Services;

public class EventSearchQueryStringBuilder : IEventSearchQueryStringBuilder
{
    public List<SelectedFilter> BuildEventSearchFilters(EventFilterChoices eventFilterChoices, IUrlHelper url)
    {
        var queryParameters = BuildQueryParameters(eventFilterChoices);

        var filters = new List<SelectedFilter>();

        AddDateSelectedFilters(eventFilterChoices, url, queryParameters, filters);
        AddEventFormatSelectedFilters(eventFilterChoices, url, queryParameters, filters);
        AddEventTypeSelectedFilters(eventFilterChoices, url, queryParameters, filters);
        AddRegionSelectedFilters(eventFilterChoices, url, queryParameters, filters);
        return filters;
    }

    private static void AddDateSelectedFilters(EventFilterChoices eventFilterChoices, IUrlHelper url, List<string> queryParameters,
        ICollection<SelectedFilter> filters)
    {
        var filterFromDate = AddFilterDateTime("From date", FilterFields.FromDate, eventFilterChoices.FromDate,
            queryParameters, filters.Count + 1, eventFilterChoices, url);
        if (filterFromDate != null) filters.Add(filterFromDate);

        var filterToDate = AddFilterDateTime("To date", FilterFields.ToDate, eventFilterChoices.ToDate, queryParameters, filters.Count + 1,
            eventFilterChoices, url);
        if (filterToDate != null) filters.Add(filterToDate);
    }

    private static void AddEventFormatSelectedFilters(EventFilterChoices eventFilterChoices,
        IUrlHelper url, List<string> queryParameters, ICollection<SelectedFilter> filters)
    {
        if (!eventFilterChoices.EventFormats.Any()) return;

        var eventFormatsChecklistFiltered = new List<ChecklistLookup>();
        foreach (var a in eventFilterChoices.EventFormatChecklistDetails.Lookups)
        {
            eventFormatsChecklistFiltered.AddRange(from b in eventFilterChoices.EventFormats
                                                   where a.Value == b.ToString()
                                                   select a);
        }

        foreach (var lookup in from lookup in eventFilterChoices.EventFormatChecklistDetails.Lookups from ef in eventFilterChoices.EventFormats!.Where(ef => ef.ToString() == lookup.Value) select lookup)
        {
            lookup.Checked = true;
        }

        var filterEventFormat = AddFilterChecklist("Event format", FilterFields.EventFormat,
            eventFormatsChecklistFiltered,
            queryParameters, filters.Count + 1, eventFilterChoices, url);
        if (filterEventFormat != null) filters.Add(filterEventFormat);
    }

    private static void AddEventTypeSelectedFilters(EventFilterChoices eventFilterChoices, IUrlHelper url,
        List<string> queryParameters, ICollection<SelectedFilter> filters)
    {
        if (!eventFilterChoices.CalendarIds.Any()) return;

        var eventTypesChecklistFiltered = new List<ChecklistLookup>();
        foreach (var a in eventFilterChoices.EventTypeChecklistDetails.Lookups)
        {
            eventTypesChecklistFiltered.AddRange(from b in eventFilterChoices.CalendarIds
                                                 where a.Value == b.ToString()
                                                 select a);
        }

        //MFCMFC
        foreach (var lookup in from lookup in eventFilterChoices.EventTypeChecklistDetails.Lookups from ef in eventFilterChoices.CalendarIds!.Where(ef => ef.ToString() == lookup.Value) select lookup)
        {
            lookup.Checked = true;
        }

        var filterEventType = AddFilterChecklist("Event type", FilterFields.EventType,
            eventTypesChecklistFiltered,
            queryParameters, filters.Count + 1, eventFilterChoices, url);
        if (filterEventType != null) filters.Add(filterEventType);
    }

    private static void AddRegionSelectedFilters(EventFilterChoices eventFilterChoices, IUrlHelper url,
        List<string> queryParameters, ICollection<SelectedFilter> filters)
    {
        if (!eventFilterChoices.RegionIds.Any()) return;

        var regionsChecklistFilter = new List<ChecklistLookup>();
        foreach (var a in eventFilterChoices.RegionChecklistDetails.Lookups)
        {
            regionsChecklistFilter.AddRange(from b in eventFilterChoices.RegionIds
                                            where a.Value == b.ToString()
                                            select a);
        }

        //MFCMFC NEW ADDITION
        foreach (var lookup in from lookup in eventFilterChoices.RegionChecklistDetails.Lookups from ef in eventFilterChoices.RegionIds!.Where(ef => ef.ToString() == lookup.Value) select lookup)
        {
            lookup.Checked = true;
        }

        var filterRegion = AddFilterChecklist("Regions", FilterFields.Region,
            regionsChecklistFilter,
            queryParameters, filters.Count + 1, eventFilterChoices, url);
        if (filterRegion != null) filters.Add(filterRegion);
    }

    private static List<string> BuildQueryParameters(EventFilterChoices eventFilterChoices)
    {
        var queryParameters = new List<string>();

        if (eventFilterChoices.FromDate != null)
        {
            queryParameters.Add("fromDate=" + eventFilterChoices.FromDate.Value.ToApiString());
        }

        if (eventFilterChoices.ToDate != null)
        {
            queryParameters.Add("toDate=" + eventFilterChoices.ToDate.Value.ToApiString());
        }

        queryParameters.AddRange(eventFilterChoices.EventFormats.Select(eventFormat => "eventFormat=" + eventFormat));
        queryParameters.AddRange(eventFilterChoices.CalendarIds.Select(eventType => "calendarId=" + eventType));
        queryParameters.AddRange(eventFilterChoices.RegionIds.Select(region => "regionId=" + region));

        return queryParameters;
    }

    private static SelectedFilter? AddFilterDateTime(string fieldName, FilterFields filterToRemove, DateTime? eventFilter, List<string> queryParameters, int filterFieldOrder, EventFilterChoices eventFilterChoices, IUrlHelper url)
    {
        if (eventFilter == null) return null;

        return
             new SelectedFilter
             {
                 FieldName = fieldName,
                 FieldOrder = filterFieldOrder,
                 Filters = new List<EventFilterItem>
                 {
                    new()
                    {
                        ClearFilterLink = BuildQueryString(queryParameters, filterToRemove, eventFilterChoices,null,url), Order = 1,
                        Value = eventFilter.Value.ToScreenString()
                    }
                 }
             };
    }

    private static SelectedFilter? AddFilterChecklist(string fieldName, FilterFields filterToRemove, List<ChecklistLookup> filterValues, List<string> queryParameters, int filterFieldOrder, EventFilterChoices eventFilterChoices, IUrlHelper url)
    {
        var order = 0;
        var selectedFilter =
            new SelectedFilter
            {
                FieldName = fieldName,
                FieldOrder = filterFieldOrder,
                Filters = new List<EventFilterItem> { }
            };

        var filtersToAdd = new List<EventFilterItem>();

        if (filterToRemove is FilterFields.EventFormat or FilterFields.EventType or FilterFields.Region)
        {
            foreach (var filterValue in filterValues)
            {
                order++;
                filtersToAdd.Add(new EventFilterItem
                {
                    ClearFilterLink = BuildQueryString(queryParameters, filterToRemove, eventFilterChoices, filterValue, url),
                    Order = order,
                    Value = filterValue.Name
                });
            }
        }

        selectedFilter.Filters = filtersToAdd;

        return selectedFilter;
    }

    private static string? BuildQueryString(List<string> queryParameters, FilterFields filterToRemove, EventFilterChoices eventFilterChoices, ChecklistLookup? filterValue, IUrlHelper url)
    {
        var queryParametersToBuild = new List<string>();

        foreach (var p in queryParameters)
        {
            switch (filterToRemove)
            {
                case FilterFields.FromDate when p == "fromDate=" + eventFilterChoices.FromDate!.Value.ToApiString():
                case FilterFields.ToDate when p == "toDate=" + eventFilterChoices.ToDate!.Value.ToApiString():
                    continue;
                case FilterFields.EventFormat:
                    ExcludeQueryParametersForMatchingEventFormats(eventFilterChoices, filterValue, p, queryParametersToBuild);
                    break;
                case FilterFields.EventType:
                    ExcludeQueryParametersForMatchingEventTypes(eventFilterChoices, filterValue, p, queryParametersToBuild);
                    break;
                case FilterFields.Region:
                    ExcludeQueryParametersForMatchingRegions(eventFilterChoices, filterValue, p, queryParametersToBuild);
                    break;
                default:
                    queryParametersToBuild.Add(p);
                    break;
            }
        }
        return queryParametersToBuild.Any() ? $"{url.RouteUrl(RouteNames.NetworkEvents)}?{string.Join('&', queryParametersToBuild)}" : url.RouteUrl(RouteNames.NetworkEvents);
    }

    private static void ExcludeQueryParametersForMatchingEventFormats(EventFilterChoices eventFilterChoices,
        ChecklistLookup? filterValue, string p, ICollection<string> queryParametersToBuild)
    {
        var addParameter = true;
        if (filterValue != null)
        {
            foreach (var choice in eventFilterChoices.EventFormats
                         .Where(choice => choice.ToString() == filterValue.Value)
                         .Where(choice => p == "eventFormat=" + filterValue.Value))
            {
                addParameter = false;
            }
        }

        if (addParameter)
        {
            queryParametersToBuild.Add(p);
        }
    }

    private static void ExcludeQueryParametersForMatchingEventTypes(EventFilterChoices eventFilterChoices,
        ChecklistLookup? filterValue, string p, ICollection<string> queryParametersToBuild)
    {
        var addParameter = true;

        if (filterValue != null)
        {
            foreach (var choice in eventFilterChoices.CalendarIds
                         .Where(choice => choice.ToString() == filterValue.Value)
                         .Where(choice => p == "calendarId=" + filterValue.Value))
            {
                addParameter = false;
            }
        }

        if (addParameter)
        {
            queryParametersToBuild.Add(p);
        }
    }

    private static void ExcludeQueryParametersForMatchingRegions(EventFilterChoices eventFilterChoices,
        ChecklistLookup? filterValue, string p, ICollection<string> queryParametersToBuild)
    {
        var addParameter = true;

        foreach (var choice in eventFilterChoices.RegionIds.Where(choice => choice.ToString() == filterValue?.Value)
                     .Where(choice => p == "regionId=" + filterValue?.Value))
        {
            addParameter = false;
        }

        if (addParameter)
        {
            queryParametersToBuild.Add(p);
        }
    }
}