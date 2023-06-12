using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Models;

public class CalendarViewModel
{
    public const int TotalCalendarDaysNormal = 35;
    public const int TotalCalendarDaysExtended = 42;

    private readonly IUrlHelper _urlHelper;
    private readonly DateOnly _today;

    public DateOnly FirstDayOfCurrentMonth { get; init; }
    public List<CalendarItem> CalendarItems { get; }

    public string PreviousMonthLink => _urlHelper.RouteUrl(RouteNames.EventsHub, new { FirstDayOfCurrentMonth.AddMonths(-1).Month, FirstDayOfCurrentMonth.AddMonths(-1).Year })!;


    public string NextMonthLink => _urlHelper.RouteUrl(RouteNames.EventsHub, new { FirstDayOfCurrentMonth.AddMonths(1).Month, FirstDayOfCurrentMonth.AddMonths(1).Year })!;

    public CalendarViewModel(int month, int year, IUrlHelper urlHelper, DateOnly today)
    {
        _urlHelper = urlHelper;
        _today = today;
        FirstDayOfCurrentMonth = new DateOnly(year, month, 1);
        CalendarItems = RenderCalendarItems();
    }

    private List<CalendarItem> RenderCalendarItems()
    {
        List<CalendarItem> calendar = new();

        int firstDayOfTheWeek = (int)FirstDayOfCurrentMonth.DayOfWeek;
        var startIndex = firstDayOfTheWeek == 0 ? 6 : firstDayOfTheWeek - 1;
        var currentRenderingDay = FirstDayOfCurrentMonth;
        var noOfDaysToRender = GetNumberOfDaysToRender(startIndex);

        for (int i = 0; i < noOfDaysToRender; i++)
        {
            if (i < startIndex || currentRenderingDay.Month != FirstDayOfCurrentMonth.Month)
            {
                calendar.Add(new(i, null, false));
                continue;
            }

            calendar.Add(new(i, currentRenderingDay, currentRenderingDay == _today));
            currentRenderingDay = currentRenderingDay.AddDays(1);
        }
        return calendar;
    }

    private int GetNumberOfDaysToRender(int firstDayOfTheWeek)
    {
        var daysInMonth = DateTime.DaysInMonth(FirstDayOfCurrentMonth.Year, FirstDayOfCurrentMonth.Month);
        var totalDays = (firstDayOfTheWeek + daysInMonth);
        return totalDays > TotalCalendarDaysNormal ? TotalCalendarDaysExtended : TotalCalendarDaysNormal;
    }
}

public record CalendarItem(int Index, DateOnly? Day, bool IsToday);
