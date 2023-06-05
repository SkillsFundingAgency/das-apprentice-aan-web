using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Extensions;

public static class CalendarEventSummaryExtensions
{
    public static string GetEventLink(this CalendarEventSummary calendarEventSummary, IUrlHelper helper)
    {
        return helper.RouteUrl(RouteNames.NetworkEventDetails, new { id = calendarEventSummary.CalendarEventId })!;
    }
}
