namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
public class JoinTheNetworkViewModel: JoinTheNetworkSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
}

public class JoinTheNetworkSubmitModel
{
    public int maxWordCount { get; } = 250;
    public string? ReasonForJoiningTheNetwork { get; set; }
}