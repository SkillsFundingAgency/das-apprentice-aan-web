namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

public class EmployerDetailsViewModel : EmployerDetailsSubmitModel, IBackLink
{
    public const string AddDetailsHeader = "Add the details of your employer";
    public const string CheckDetailsHeader = "Check the details of your employer";
    public string BackLink { get; set; } = null!;
    public string PageHeader => string.IsNullOrWhiteSpace(Postcode) ? AddDetailsHeader : CheckDetailsHeader;
}

public class EmployerDetailsSubmitModel
{
    public string? EmployerName { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? County { get; set; }
    public string? Town { get; set; }
    public string? Postcode { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }

}