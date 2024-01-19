namespace SFA.DAS.Aan.SharedUi.Models.LeaveTheNetwork;

public class SubmitLeaveTheNetworkViewModel
{
    public List<LeavingReasonModel>? LeavingReasons { get; set; }
    public List<LeavingReasonModel>? LeavingExperience { get; set; }
    public List<LeavingReasonModel>? LeavingBenefits { get; set; }

    public int SelectedLeavingExperienceRating { get; set; } = 0;
}