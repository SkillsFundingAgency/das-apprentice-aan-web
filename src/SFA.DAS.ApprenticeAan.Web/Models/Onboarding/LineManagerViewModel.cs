namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
public class LineManagerViewModel: LineManagerSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
}

public class LineManagerSubmitModel
{
    public bool? HasEmployersApproval { get; set; }
}