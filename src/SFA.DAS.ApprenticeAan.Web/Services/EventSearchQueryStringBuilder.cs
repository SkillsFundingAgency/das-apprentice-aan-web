using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.Models;
using SFA.DAS.ApprenticeAan.Web.HtmlHelpers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Services;

public class EventSearchQueryStringBuilder : IEventSearchQueryStringBuilder
{
    public List<SelectedFilter> BuildEventSearchFilters(EventFilterChoices eventFilterChoices,
        List<ChecklistLookup> eventTypesLookup, IUrlHelper url)
    {
        var queryParameters = BuildQueryParameters(eventFilterChoices);

        var filters = new List<SelectedFilter>();

        var filterFromDate = AddFilterDateTime("From date", FilterFields.FromDate, eventFilterChoices.FromDate, queryParameters, 1, eventFilterChoices, url);
        if (filterFromDate != null) filters.Add(filterFromDate);

        var filterToDate = AddFilterDateTime("To date", FilterFields.ToDate, eventFilterChoices.ToDate, queryParameters, 2, eventFilterChoices, url);
        if (filterToDate != null) filters.Add(filterToDate);

        if (eventFilterChoices.EventFormats != null && eventFilterChoices.EventFormats.Any())
        {
            var eventFormatsCheckList = new List<ChecklistLookup>
            {
                new ChecklistLookup("In person", EventFormat.InPerson.ToString()),
                new ChecklistLookup("Online", EventFormat.Online.ToString()),
                new ChecklistLookup("Hybrid", EventFormat.Hybrid.ToString())
            };

            var filterEventFormat = AddFilterChecklist("Event format", FilterFields.EventFormat, eventFormatsCheckList,
                queryParameters, 3, eventFilterChoices, url);
            if (filterEventFormat != null) filters.Add(filterEventFormat);
        }

        //
        // var filterEventTypes = AddFilterEventTypes(eventFilterChoices.CalendarIds, 4, eventFilterChoices, eventTypesLookup, url);
        // if (filterEventTypes != null)
        //     filters.Add(filterEventTypes);
        return filters;
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


    private static SelectedFilter? AddFilterChecklist(string fieldName, FilterFields filterToRemove, List<ChecklistLookup>? filterValues, List<string> queryParameters, int filterFieldOrder, EventFilterChoices eventFilterChoices, IUrlHelper url)
    {
        if (filterValues == null || !filterValues.Any()) return null;

        var order = 0;
        var selectedFilter =
            new SelectedFilter
            {
                FieldName = fieldName,
                FieldOrder = filterFieldOrder,
                Filters = new List<FilterItem> { }
            };

        var filtersToAdd = new List<FilterItem>();

        if (filterToRemove == FilterFields.EventFormat)
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


        // new()
        // {
        //     ClearFilterLink = BuildQueryString(queryParameters, filterToRemove, eventFilterChoices, url),
        //     Order = 1,
        //     Value = DateTimeHelper.ToScreenFormat(eventFilter)
        // }

        return selectedFilter;
    }

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

    private static string? BuildQueryString(List<string> queryParameters, FilterFields filterToRemove, EventFilterChoices? eventFilterChoices, ChecklistLookup? filterValue, IUrlHelper url) //, List<ChecklistLookup> eventTypeLookp)
    {
        //var queryParameters = BuildQueryParametersForDates(queryParameters, eventFilterChoices);
        //queryParameters.AddRange(BuildQueryParametersForEventFormats(removeFilter, eventFilterChoices));

        if (eventFilterChoices == null)
            return string.Join('&', queryParameters);

        var queryParametersToBuild = new List<string>();

        foreach (var p in queryParameters)
        {
            switch (filterToRemove)
            {
                case FilterFields.FromDate
                    when p == "fromDate=" + DateTimeHelper.ToUrlFormat(eventFilterChoices.FromDate):
                case FilterFields.ToDate when p == "toDate=" + DateTimeHelper.ToUrlFormat(eventFilterChoices.ToDate):
                    continue;
                case FilterFields.EventFormat:
                    if (eventFilterChoices?.EventFormats == null || !eventFilterChoices.EventFormats.Any() ||
                        filterValue == null)
                    {
                        continue;
                    }

                    var addParameter = true;
                    foreach (var choice in eventFilterChoices.EventFormats)
                    {
                        if (choice.ToString() == filterValue.Name)
                        {
                            if (p == "eventFormat=" + filterValue.Value)
                            {
                                addParameter = false;
                            }
                        }


                    }

                    if (addParameter)
                    {
                        queryParametersToBuild.Add(p);
                    }

                    break;
                default:
                    queryParametersToBuild.Add(p);
                    break;
            }
        }
        return queryParametersToBuild.Any() ? $"{url.RouteUrl(RouteNames.NetworkEvents)}?{string.Join('&', queryParametersToBuild)}" : url.RouteUrl(RouteNames.NetworkEvents);
    }

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

    // private static List<string> BuildQueryParametersForDates(List<string> queryParameters, EventFilterChoices eventFilterChoices)
    // {
    //   //  var queryParameters = new List<string>();
    //
    //     if (eventFilterChoices.FromDate != null)
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