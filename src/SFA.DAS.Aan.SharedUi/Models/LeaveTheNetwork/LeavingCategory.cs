namespace SFA.DAS.Aan.SharedUi.Models.LeaveTheNetwork;

public class LeavingCategory
{
    public string Category { get; set; } = null!;

    public List<LeavingReasonModel> LeavingReasons { get; set; } = null!;
}