namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

public class EmployerSearchViewModel : EmployerSearchSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!; // add this to submit model
    public string ManualEntryLink { get; set; } = null!;
}

public class EmployerSearchSubmitModel
{
    public string? SearchTerm { get; set; }

    // Fields to be updated on selection of an address from the drop down
    public string? OrganisationName { get; set; }
    public string? Town { get; set; }
    public string? County { get; set; }
    public string? Postcode { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
}