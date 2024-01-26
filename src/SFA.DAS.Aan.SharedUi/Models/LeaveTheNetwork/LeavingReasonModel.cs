namespace SFA.DAS.Aan.SharedUi.Models.LeaveTheNetwork;

public class LeavingReasonModel
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Ordering { get; set; }
    public bool IsSelected { get; set; }
}