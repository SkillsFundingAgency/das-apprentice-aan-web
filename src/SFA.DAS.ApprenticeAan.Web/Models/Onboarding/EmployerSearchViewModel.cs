namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

public class EmployerSearchViewModel : EmployerSearchSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
    public string ManualEntryLink { get; set; } = null!;
}

public class EmployerSearchSubmitModel
{
    public string? SearchTerm { get; set; }
    public string? OrganisationName { get; set; }
    public string? Town { get; set; }
    public string? County { get; set; }
    public string? Postcode { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }

}