namespace SFA.DAS.Aan.SharedUi.Models.LeaveTheNetwork;

public class LeaveTheNetworkViewModel : SubmitLeaveTheNetworkViewModel
{
    public string ProfileSettingsLink { get; set; } = null!;
    public string LeavingReasonsTitle { get; set; } = null!;
    public string LeavingExperienceTitle { get; set; } = null!;
    public string LeavingBenefitsTitle { get; set; } = null!;
}