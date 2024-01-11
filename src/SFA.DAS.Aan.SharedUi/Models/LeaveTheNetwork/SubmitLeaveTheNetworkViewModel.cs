namespace SFA.DAS.Aan.SharedUi.Models.LeaveTheNetwork;
public class SubmitLeaveTheNetworkViewModel
{
    public string LeavingReasonsTitle { get; set; } = null!;
    public string LeavingExperienceTitle { get; set; } = null!;
    public string LeavingBenefitsTitle { get; set; } = null!;

    public List<LeavingReasonModel>? LeavingReasons { get; set; } = null!;
    public List<LeavingReasonModel>? LeavingExperience { get; set; } = null!;
    public List<LeavingReasonModel>? LeavingBenefits { get; set; } = null!;

    public int LeavingExperienceRating { get; set; } = 0;
}