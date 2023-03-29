namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
public class JoinTheNetworkViewModel: JoinTheNetworkSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
}

public class JoinTheNetworkSubmitModel
{
    public string? ReasonForJoiningTheNetwork { get; set; }
}