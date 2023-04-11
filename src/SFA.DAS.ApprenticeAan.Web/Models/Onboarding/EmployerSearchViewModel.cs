namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

public class EmployerSearchViewModel : EmployerSearchSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!; // add this to submit model
    public string ManualEntryLink { get; set; } = null!;
}

public class EmployerSearchSubmitModel
{
    public string? SearchTerm { get; set; }
}