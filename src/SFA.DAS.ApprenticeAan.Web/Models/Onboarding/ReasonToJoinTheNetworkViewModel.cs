namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
public class ReasonToJoinTheNetworkViewModel: ReasonToJoinTheNetworkSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
}

public class ReasonToJoinTheNetworkSubmitModel
{
    public bool? EngagedWithAPreviousAmbassadorInTheNetwork { get; set; }
}