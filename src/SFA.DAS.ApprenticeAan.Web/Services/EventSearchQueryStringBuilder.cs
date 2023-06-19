using Microsoft.AspNetCore.Mvc;
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

        return filters;
    }

    private static string? BuildQueryString(FilterFields removeFilter, EventFilters eventFilters, IUrlHelper url)
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
};