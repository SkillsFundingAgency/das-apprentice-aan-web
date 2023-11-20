using SFA.DAS.Aan.SharedUi.Extensions;
using SFA.DAS.Aan.SharedUi.Models.NetworkEvents;

namespace SFA.DAS.ApprenticeAan.Web.Services;

public static class QueryStringParameterBuilder
{

    public static Dictionary<string, string[]> BuildQueryStringParameters(GetNetworkEventsRequest request)
    {
        var parameters = new Dictionary<string, string[]>();
        if (!string.IsNullOrWhiteSpace(request.Keyword)) parameters.Add("keyword", new string[] { request.Keyword.Trim() });
        if (request.FromDate != null) parameters.Add("fromDate", new string[] { request.FromDate.Value.ToApiString() });
        if (request.ToDate != null) parameters.Add("toDate", new string[] { request.ToDate.Value.ToApiString() });
        parameters.Add("eventFormat", request.EventFormat.Select(format => format.ToString()).ToArray());
        parameters.Add("calendarId", request.CalendarId.Select(cal => cal.ToString()).ToArray());
        parameters.Add("regionId", request.RegionId.Select(region => region.ToString()).ToArray());
        if (request.Page != null) parameters.Add("page", new[] { request.Page?.ToString() }!);
        if (request.PageSize != null) parameters.Add("pageSize", new[] { request.PageSize?.ToString() }!);
        return parameters;
    }
}