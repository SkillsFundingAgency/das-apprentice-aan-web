using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class CalendarViewModel
{
    private readonly IUrlHelper _urlHelper;
    public int CurrentMonth { get; init; }
    public int CurrentYear { get; init; }

    public DateOnly FirstDayOfCurrentMonth { get; init; }

    public string PreviousMonthLink => _urlHelper.RouteUrl(RouteNames.EventsHub, new { FirstDayOfCurrentMonth.AddMonths(-1).Month, FirstDayOfCurrentMonth.AddMonths(-1).Year })!;
    public string NextMonthLink => _urlHelper.RouteUrl(RouteNames.EventsHub, new { FirstDayOfCurrentMonth.AddMonths(1).Month, FirstDayOfCurrentMonth.AddMonths(1).Year })!;

    public CalendarViewModel(int month, int year, IUrlHelper urlHelper)
    {
        CurrentMonth = month;
        CurrentYear = year;
        _urlHelper = urlHelper;
        FirstDayOfCurrentMonth = new DateOnly(year, month, 1);
    }
}
