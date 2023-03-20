namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
public class CurrentJobTitleViewModel : CurrentJobTitleSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
}

public class CurrentJobTitleSubmitModel
{
    public string? EnteredJobTitle { get; set; }
}