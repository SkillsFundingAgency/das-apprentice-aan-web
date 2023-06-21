using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.Models;
using SFA.DAS.ApprenticeAan.Web.HtmlHelpers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Services;

public class EventSearchQueryStringBuilder : IEventSearchQueryStringBuilder
{
    public List<SelectedFilter> BuildEventSearchFilters(EventFilterChoices eventFilterChoices,
        List<ChecklistLookup> eventFormatsLookup, List<ChecklistLookup> eventTypesLookup, IUrlHelper url)
    {
        var queryParameters = BuildQueryParameters(eventFilterChoices);

        var filters = new List<SelectedFilter>();

        AddDateSelectedFilters(eventFilterChoices, url, queryParameters, filters);
        AddEventFormatSelectedFilters(eventFilterChoices, eventFormatsLookup, url, queryParameters, filters);
        AddEventTypeSelectedFilters(eventFilterChoices, eventTypesLookup, url, queryParameters, filters);

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

    private static void AddEventFormatSelectedFilters(EventFilterChoices eventFilterChoices, List<ChecklistLookup> eventFormatsLookup,
        IUrlHelper url, List<string> queryParameters, ICollection<SelectedFilter> filters)
    {
        if (eventFilterChoices.EventFormats != null && eventFilterChoices.EventFormats.Any())
        {
            var eventFormatsChecklistFiltered = new List<ChecklistLookup>();
            foreach (var a in eventFormatsLookup)
            {
                eventFormatsChecklistFiltered.AddRange(from b in eventFilterChoices.EventFormats
                                                       where a.Value == b.ToString()
                                                       select a);
            }

            var filterEventFormat = AddFilterChecklist("Event format", FilterFields.EventFormat,
                eventFormatsChecklistFiltered,
                queryParameters, filters.Count + 1, eventFilterChoices, url);
            if (filterEventFormat != null) filters.Add(filterEventFormat);
        }
    }

    private static void AddEventTypeSelectedFilters(EventFilterChoices eventFilterChoices, List<ChecklistLookup> eventTypesLookup, IUrlHelper url,
        List<string> queryParameters, ICollection<SelectedFilter> filters)
    {
        var eventTypesChecklistFiltered = new List<ChecklistLookup>();
        if (eventFilterChoices.CalendarIds != null)
        {
            foreach (var a in eventTypesLookup)
            {
                eventTypesChecklistFiltered.AddRange(from b in eventFilterChoices.CalendarIds
                                                     where a.Value == b.ToString()
                                                     select a);
            }

            if (eventFilterChoices.CalendarIds.Any())
            {
                var filterEventType = AddFilterChecklist("Event type", FilterFields.EventType,
                    eventTypesChecklistFiltered,
                    queryParameters, filters.Count + 1, eventFilterChoices, url);
                if (filterEventType != null) filters.Add(filterEventType);
            }
        }
    }

    private static List<string> BuildQueryParameters(EventFilterChoices eventFilterChoices)
    {
        var queryParameters = new List<string>();

        if (eventFilterChoices.FromDate != null)
        {
            queryParameters.Add("fromDate=" + DateTimeHelper.ToUrlFormat(eventFilterChoices.FromDate));
        }

        if (eventFilterChoices.ToDate != null)
        {
            queryParameters.Add("toDate=" + DateTimeHelper.ToUrlFormat(eventFilterChoices.ToDate));
        }

        if (eventFilterChoices.EventFormats != null)
            queryParameters.AddRange(
                eventFilterChoices.EventFormats.Select(eventFormat => "eventFormat=" + eventFormat));

        if (eventFilterChoices.CalendarIds != null)
            queryParameters.AddRange(eventFilterChoices.CalendarIds.Select(eventFormat => "calendarId=" + eventFormat));
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
                 Filters = new List<FilterItem>
                 {
                    new()
                    {
                        ClearFilterLink = BuildQueryString(queryParameters, filterToRemove, eventFilterChoices,null,url), Order = 1,
                        Value = DateTimeHelper.ToScreenFormat(eventFilter)
                    }
                 }
             };
    }

    private static SelectedFilter? AddFilterChecklist(string fieldName, FilterFields filterToRemove, List<ChecklistLookup> filterValues, List<string> queryParameters, int filterFieldOrder, EventFilterChoices eventFilterChoices, IUrlHelper url)
    {
        if (!filterValues.Any()) return null;

        var order = 0;
        var selectedFilter =
            new SelectedFilter
            {
                FieldName = fieldName,
                FieldOrder = filterFieldOrder,
                Filters = new List<FilterItem> { }
            };

        var filtersToAdd = new List<FilterItem>();

        if (filterToRemove is FilterFields.EventFormat or FilterFields.EventType)
        {
            foreach (var filterValue in filterValues)
            {
                order++;
                filtersToAdd.Add(new FilterItem
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
                case FilterFields.FromDate when p == "fromDate=" + DateTimeHelper.ToUrlFormat(eventFilterChoices!.FromDate):
                case FilterFields.ToDate when p == "toDate=" + DateTimeHelper.ToUrlFormat(eventFilterChoices!.ToDate):
                    continue;
                case FilterFields.EventFormat:
                    ExcludeQueryParametersForMatchingEventFormats(eventFilterChoices, filterValue, p, queryParametersToBuild);
                    break;
                case FilterFields.EventType:
                    ExcludeQueryParametersForMatchingEventTypes(eventFilterChoices, filterValue, p, queryParametersToBuild);
                    break;
                default:
                    queryParametersToBuild.Add(p);
                    break;
            }
        }
        return queryParametersToBuild.Any() ? $"{url.RouteUrl(RouteNames.NetworkEvents)}?{string.Join('&', queryParametersToBuild)}" : url.RouteUrl(RouteNames.NetworkEvents);
    }

    private static void ExcludeQueryParametersForMatchingEventFormats(EventFilterChoices eventFilterChoices,
        ChecklistLookup filterValue, string p, ICollection<string> queryParametersToBuild)
    {
        if (eventFilterChoices.EventFormats == null || !eventFilterChoices.EventFormats.Any())
        {
            return;
        }

        var addParameter = true;
        foreach (var choice in eventFilterChoices.EventFormats.Where(choice => choice.ToString() == filterValue.Value)
                     .Where(choice => p == "eventFormat=" + filterValue.Value))
        {
            addParameter = false;
        }

        if (addParameter)
        {
            queryParametersToBuild.Add(p);
        }
    }

    private static void ExcludeQueryParametersForMatchingEventTypes(EventFilterChoices eventFilterChoices,
        ChecklistLookup filterValue, string p, ICollection<string> queryParametersToBuild)
    {
        if (eventFilterChoices.CalendarIds == null || !eventFilterChoices.CalendarIds.Any())
        {
            return;
        }

        var addParameter = true;

        foreach (var choice in eventFilterChoices.CalendarIds.Where(choice => choice.ToString() == filterValue.Value)
                     .Where(choice => p == "calendarId=" + filterValue.Value))
        {
            addParameter = false;
        }

        if (addParameter)
        {
            queryParametersToBuild.Add(p);
        }
    }
}