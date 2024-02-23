namespace SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;

public class SubmitAttendanceCommand
{
    public bool NewStatus { get; set; }
    public DateTime StartDateTime { get; set; }
}
