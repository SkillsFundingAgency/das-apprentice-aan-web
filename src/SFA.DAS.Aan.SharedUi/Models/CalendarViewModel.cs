//using SFA.DAS.Aan.SharedUi.Infrastructure;

namespace SFA.DAS.Aan.SharedUi.Models;

public class CalendarViewModel
{
    public const int TotalCalendarDaysNormal = 35;
    public const int TotalCalendarDaysExtended = 42;

    private readonly DateOnly _today;

    public DateOnly FirstDayOfCurrentMonth { get; init; }
    public List<CalendarItem> CalendarItems { get; }

    public string PreviousMonthLink { get; set; }

    public string NextMonthLink { get; set; }

    public CalendarViewModel(DateOnly firstDayOfTheMonth, DateOnly today, IEnumerable<Appointment> appointments)
    {
        _today = today;
        FirstDayOfCurrentMonth = firstDayOfTheMonth;
        CalendarItems = RenderCalendarItems(appointments);
    }

    private List<CalendarItem> RenderCalendarItems(IEnumerable<Appointment> appointments)
    {
        List<CalendarItem> calendar = new();

        int firstDayOfTheWeek = (int)FirstDayOfCurrentMonth.DayOfWeek;
        var startIndex = firstDayOfTheWeek == 0 ? 6 : firstDayOfTheWeek - 1;
        var currentRenderingDay = FirstDayOfCurrentMonth;
        var noOfItemsToRender = GetNumberOfItemsToRender(startIndex);

        for (int i = 0; i < noOfItemsToRender; i++)
        {
            if (i < startIndex || currentRenderingDay.Month != FirstDayOfCurrentMonth.Month)
            {
                calendar.Add(new(i, null, false, new()));
                continue;
            }

            calendar.Add(new(i, currentRenderingDay, currentRenderingDay == _today, appointments.Where(a => a.Date == currentRenderingDay).ToList()));
            currentRenderingDay = currentRenderingDay.AddDays(1);
        }
        return calendar;
    }

    private int GetNumberOfItemsToRender(int firstDayOfTheWeek)
    {
        var daysInMonth = DateTime.DaysInMonth(FirstDayOfCurrentMonth.Year, FirstDayOfCurrentMonth.Month);
        var totalDays = firstDayOfTheWeek + daysInMonth;
        return totalDays > TotalCalendarDaysNormal ? TotalCalendarDaysExtended : TotalCalendarDaysNormal;
    }
}

public record CalendarItem(int Index, DateOnly? Day, bool IsToday, List<Appointment> Appointments);

public record Appointment(string Title, string Url, DateOnly Date, string Format);

