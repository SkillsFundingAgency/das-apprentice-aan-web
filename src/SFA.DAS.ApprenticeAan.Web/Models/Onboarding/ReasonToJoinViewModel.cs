namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
public class ReasonToJoinViewModel : ReasonToJoinSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
}

public class ReasonToJoinSubmitModel
{
    public int MaxWordCount { get; } = 250;
    public string? ReasonForJoiningTheNetwork { get; set; }
}