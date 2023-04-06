namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
public class PreviousEngagementViewModel : PreviousEngagementSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
}

public class PreviousEngagementSubmitModel
{
    public bool? EngagedWithAPreviousAmbassadorInTheNetwork { get; set; }
}