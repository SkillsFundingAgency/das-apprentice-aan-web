namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class SubmitAttendanceCommand
{
    public Guid CalendarEventId { get; set; }
    public bool NewStatus { get; set; }
    public DateTime StartTimeAndDate { get; set; }
}
