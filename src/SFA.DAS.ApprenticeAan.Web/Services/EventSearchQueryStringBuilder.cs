using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.Models;
using SFA.DAS.ApprenticeAan.Web.HtmlHelpers;

namespace SFA.DAS.ApprenticeAan.Web.Services;

public class EventSearchQueryStringBuilder : IEventSearchQueryStringBuilder
{
    public List<SelectedFilter> BuildEventSearchFilters(EventFilterChoices eventFilterChoices,
        List<ChecklistLookup> eventTypesLookup, IUrlHelper url)
    {
        var queryParameters = BuildQueryParameters(eventFilterChoices);

        var filters = new List<SelectedFilter>();

        // var filterFromDate = AddFilterDateTime("From date", FilterFields.FromDate, eventFilterChoices.FromDate, 1, eventFilterChoices, url);
        // if (filterFromDate != null)
        //     filters.Add(filterFromDate);
        //
        // var filterToDate = AddFilterDateTime("To date", FilterFields.ToDate, eventFilterChoices.ToDate, 2, eventFilterChoices, url);
        // if (filterToDate != null)
        //     filters.Add(filterToDate);
        //
        // var filterEventFormats = AddFilterEventFormats(eventFilterChoices.EventFormats, 3, eventFilterChoices, url);
        // if (filterEventFormats != null)
        //     filters.Add(filterEventFormats);
        //
        // var filterEventTypes = AddFilterEventTypes(eventFilterChoices.CalendarIds, 4, eventFilterChoices, eventTypesLookup, url);
        // if (filterEventTypes != null)
        //     filters.Add(filterEventTypes);
        return filters;
    }

    // private static SelectedFilter? AddFilterDateTime(string fieldName, FilterFields filterFields, DateTime? eventFilter, int filterFieldOrder, EventFilterChoices eventFilterChoices, IUrlHelper url)
    // {
    //     if (eventFilter == null) return null;
    //
    //     return
    //          new SelectedFilter
    //          {
    //              FieldName = fieldName,
    //              FieldOrder = filterFieldOrder,
    //              Filters = new List<FilterItem>
    //              {
    //                 new()
    //                 {
    //                     ClearFilterLink = BuildQueryString(filterFields, eventFilterChoices,url), Order = 1,
    //                     Value = DateTimeHelper.ToScreenFormat(eventFilter)
    //                 }
    //              }
    //          };
    // }

    // private static SelectedFilter? AddFilterEventFormats(List<EventFormat>? eventFormats, int filterFieldOrder, EventFilterChoices eventFilterChoices, IUrlHelper url)
    // {
    //     if (eventFormats == null || !eventFormats.Any()) return null;
    //
    //     var selectedFilter = new SelectedFilter
    //     {
    //         FieldName = "Event format",
    //         FieldOrder = filterFieldOrder
    //     };
    //
    //     var order = 0;
    //
    //     var filterItems = new List<FilterItem>();
    //
    //     foreach (var eventFormat in eventFormats)
    //     {
    //         order++;
    //
    //         var filterField = eventFormat switch
    //         {
    //             EventFormat.Online => FilterFields.EventFormatOnline,
    //             EventFormat.Hybrid => FilterFields.EventFormatHybrid,
    //             EventFormat.InPerson => FilterFields.EventFormatInPerson,
    //             _ => new FilterFields()
    //         };
    //
    //         var filterItem = new FilterItem
    //         {
    //             ClearFilterLink = BuildQueryString(filterField, eventFilterChoices, url),
    //             Order = order,
    //             Value = eventFormat.GetDescription()
    //         };
    //
    //         filterItems.Add(filterItem);
    //     }
    //
    //     selectedFilter.Filters = filterItems;
    //     return selectedFilter;
    // }

    // private static SelectedFilter? AddFilterEventTypes(List<int>? selectedValues, int filterFieldOrder, EventFilterChoices eventFilterChoices, List<ChecklistLookup> lookup, IUrlHelper url)
    // {
    //     // selectedValues = selected items for calendarId
    //     // filters - growing list of selected filters (all of them so far)
    //     // eventFilterChoices = all the event filters 
    //     if (selectedValues == null || !selectedValues.Any()) return null;
    //     var selectedFilter = new SelectedFilter
    //     {
    //         FieldName = "Event type",
    //         FieldOrder = filterFieldOrder
    //     };
    //
    //     var order = 0;
    //
    //     var filterItems = new List<FilterItem>();
    //
    //     foreach (var selectedValue in selectedValues)
    //     {
    //         order++;
    //
    //         // var filterField = eventFormat switch
    //         // {
    //         //     EventFormat.Online => FilterFields.EventFormatOnline,
    //         //     EventFormat.Hybrid => FilterFields.EventFormatHybrid,
    //         //     EventFormat.InPerson => FilterFields.EventFormatInPerson,
    //         //     _ => new FilterFields()
    //         // };
    //
    //         var filterItem = new FilterItem
    //         {
    //             ClearFilterLink = BuildQueryString(FilterFields.EventType, eventFilterChoices, url, selectedValue),
    //             Order = order,
    //             Value = selectedValue.ToString()
    //         };
    //
    //         filterItems.Add(filterItem);
    //     }
    //
    //     selectedFilter.Filters = filterItems;
    //     return selectedFilter;
    // }

    // private static string? BuildQueryString(FilterFields removeFilter, EventFilterChoices eventFilterChoices, IUrlHelper url, List<ChecklistLookup> eventTypesLookp)
    // {
    //     var queryParameters = BuildQueryParametersForDates(removeFilter, eventFilterChoices);
    //     queryParameters.AddRange(BuildQueryParametersForEventFormats(removeFilter, eventFilterChoices));
    //
    //     return queryParameters.Any() ? $"{url.RouteUrl(RouteNames.NetworkEvents)}?{string.Join('&', queryParameters)}" : url.RouteUrl(RouteNames.NetworkEvents);
    // }

    // private static IEnumerable<string> BuildQueryParametersForEventFormats(FilterFields removeFilter, EventFilterChoices eventFilterChoices)
    // {
    //     if (eventFilterChoices.EventFormats == null || !eventFilterChoices.EventFormats.Any()) return new List<string>();
    //
    //     var queryParameters = new List<string>();
    //
    //     foreach (var eventFormat in eventFilterChoices.EventFormats)
    //     {
    //         if (removeFilter != FilterFields.EventFormatOnline && eventFormat == EventFormat.Online)
    //         {
    //             queryParameters.Add("eventFormat=" + eventFormat);
    //         }
    //
    //         if (removeFilter != FilterFields.EventFormatInPerson && eventFormat == EventFormat.InPerson)
    //         {
    //             queryParameters.Add("eventFormat=" + eventFormat);
    //         }
    //
    //         if (removeFilter != FilterFields.EventFormatHybrid && eventFormat == EventFormat.Hybrid)
    //         {
    //             queryParameters.Add("eventFormat=" + eventFormat);
    //         }
    //     }
    //
    //     return queryParameters;
    // }

    // private static List<string> BuildQueryParametersForDates(FilterFields removeFilter, EventFilterChoices eventFilterChoices)
    // {
    //     var queryParameters = new List<string>();
    //
    //     if (removeFilter != FilterFields.FromDate && eventFilterChoices.FromDate != null)
    //     {
    //         queryParameters.Add("fromDate=" + DateTimeHelper.ToUrlFormat(eventFilterChoices.FromDate));
    //     }
    //
    //     if (removeFilter != FilterFields.ToDate && eventFilterChoices.ToDate != null)
    //     {
    //         queryParameters.Add("toDate=" + DateTimeHelper.ToUrlFormat(eventFilterChoices.ToDate));
    //     }
    //
    //     return queryParameters;
    // }


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
}