namespace SFA.DAS.Aan.SharedUi.Models.LeaveTheNetwork;
public class SubmitLeaveTheNetworkViewModel
{
    public List<LeavingReasonModel>? LeavingReasons { get; set; }
    public List<LeavingReasonModel>? LeavingExperience { get; set; }
    public List<LeavingReasonModel>? LeavingBenefits { get; set; }
    public string LeavingReasonsTitle { get; set; } = null!;
    public string LeavingExperienceTitle { get; set; } = null!;
    public string LeavingBenefitsTitle { get; set; } = null!;

    public int SelectedLeavingExperienceRating { get; set; } = 0;
}