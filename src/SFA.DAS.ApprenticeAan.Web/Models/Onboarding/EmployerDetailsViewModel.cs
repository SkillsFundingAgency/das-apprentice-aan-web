namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

public class EmployerDetailsViewModel : EmployerDetailsSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
}

public class EmployerDetailsSubmitModel
{
    public string? EmployerName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? County { get; set; }
    public string? Town { get; set; }
    public string? Postcode { get; set; }
}