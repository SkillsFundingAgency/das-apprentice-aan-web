using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Extensions;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.Models;
using SFA.DAS.ApprenticeAan.Web.HtmlHelpers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Services;

public class EventSearchQueryStringBuilder : IEventSearchQueryStringBuilder
{
    public List<SelectedFilter> BuildEventSearchFilters(EventFilters eventFilters, IUrlHelper url)
    {
        var filters = new List<SelectedFilter>();

        AddFilterDateTime("From date", FilterFields.FromDate, eventFilters.FromDate, filters, eventFilters, url);
        AddFilterDateTime("To date", FilterFields.ToDate, eventFilters.ToDate, filters, eventFilters, url);
        AddFilterEventFormats(eventFilters.EventFormats, filters, eventFilters, url);

        return filters;
    }

    private static string? BuildQueryString(FilterFields removeFilter, EventFilters eventFilters, IUrlHelper url)
    {
        var queryParameters = BuildQueryParametersForDates(removeFilter, eventFilters);
        queryParameters.AddRange(BuildQueryParametersForEventFormats(removeFilter, eventFilters));

        return queryParameters.Any() ? $"{url.RouteUrl(RouteNames.NetworkEvents)}?{string.Join('&', queryParameters)}" : url.RouteUrl(RouteNames.NetworkEvents);
    }

    private static IEnumerable<string> BuildQueryParametersForEventFormats(FilterFields removeFilter, EventFilters eventFilters)
    {
        if (eventFilters.EventFormats == null || !eventFilters.EventFormats.Any()) return new List<string>();

        var queryParameters = new List<string>();

        foreach (var eventFormat in eventFilters.EventFormats)
        {
            if (removeFilter != FilterFields.EventFormatOnline && eventFormat == EventFormat.Online)
            {
                queryParameters.Add("eventFormat=" + eventFormat);
            }

            if (removeFilter != FilterFields.EventFormatInPerson && eventFormat == EventFormat.InPerson)
            {
                queryParameters.Add("eventFormat=" + eventFormat);
            }

            if (removeFilter != FilterFields.EventFormatHybrid && eventFormat == EventFormat.Hybrid)
            {
                queryParameters.Add("eventFormat=" + eventFormat);
            }
        }

        return queryParameters;
    }

    private static List<string> BuildQueryParametersForDates(FilterFields removeFilter, EventFilters eventFilters)
    {
        var queryParameters = new List<string>();

        if (removeFilter != FilterFields.FromDate && eventFilters.FromDate != null)
        {
            queryParameters.Add("fromDate=" + DateTimeHelper.ToUrlFormat(eventFilters.FromDate));
        }

        if (removeFilter != FilterFields.ToDate && eventFilters.ToDate != null)
        {
            queryParameters.Add("toDate=" + DateTimeHelper.ToUrlFormat(eventFilters.ToDate));
        }

        if (eventFilters.EventFormats != null)
        {
            foreach (var eventFormat in eventFilters.EventFormats)
            {
                if (removeFilter != FilterFields.EventFormatOnline && eventFormat == EventFormat.Online)
                {
                    queryParameters.Add("eventFormat=" + eventFormat);
                }
                if (removeFilter != FilterFields.EventFormatInPerson && eventFormat == EventFormat.InPerson)
                {
                    queryParameters.Add("eventFormat=" + eventFormat);
                }
                if (removeFilter != FilterFields.EventFormatHybrid && eventFormat == EventFormat.Hybrid)
                {
                    queryParameters.Add("eventFormat=" + eventFormat);
                }
            }
        }

        return queryParameters.Any() ? $"{url.RouteUrl(RouteNames.NetworkEvents)}?{string.Join('&', queryParameters)}" : url.RouteUrl(RouteNames.NetworkEvents);
    }

    private static void AddFilterDateTime(string fieldName, FilterFields filterFields, DateTime? eventFilter, ICollection<SelectedFilter> filters, EventFilters eventFilters, IUrlHelper url)
    {
        if (eventFilter == null) return;

        filters.Add(
            new SelectedFilter
            {
                FieldName = fieldName,
                FieldOrder = filters.Count + 1,
                Filters = new List<FilterItem>
                {
                    new()
                    {
                        ClearFilterLink = BuildQueryString(filterFields, eventFilters,url), Order = 1,
                        Value = DateTimeHelper.ToScreenFormat(eventFilter)
                    }
                }
            });
    }

    private static void AddFilterEventFormats(List<EventFormat>? eventFormats, ICollection<SelectedFilter> filters, EventFilters eventFilters, IUrlHelper url)
    {
        if (eventFormats == null || !eventFormats.Any()) return;

        var selectedFilter = new SelectedFilter
        {
            FieldName = "Event format",
            FieldOrder = filters.Count + 1
        };

        var order = 0;

        var filterItems = new List<FilterItem>();

        foreach (var eventFormat in eventFormats)
        {
            order++;

            var filterField = eventFormat switch
            {
                EventFormat.Online => FilterFields.EventFormatOnline,
                EventFormat.Hybrid => FilterFields.EventFormatHybrid,
                EventFormat.InPerson => FilterFields.EventFormatInPerson,
                _ => new FilterFields()
            };

            var filterItem = new FilterItem
            {
                ClearFilterLink = BuildQueryString(filterField, eventFilters, url),
                Order = order,
                Value = eventFormat.GetDescription()
            };

            filterItems.Add(filterItem);
        }

        selectedFilter.Filters = filterItems;
        filters.Add(selectedFilter);
    }
};