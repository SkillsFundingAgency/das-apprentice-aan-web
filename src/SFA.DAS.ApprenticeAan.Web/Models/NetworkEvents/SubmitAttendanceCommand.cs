namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class SubmitAttendanceCommand
{
    public Guid CalendarEventId { get; set; }
    public bool NewStatus { get; set; }
    public DateTime StartTimeAndDate { get; set; }

    public SubmitAttendanceCommand(Guid calendarEventId, bool newStatus, DateTime startTimeAndDate)
    {
        CalendarEventId = calendarEventId;
        NewStatus = newStatus;
        StartTimeAndDate = startTimeAndDate;
    }
}
