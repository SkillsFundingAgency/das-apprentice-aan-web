namespace SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
public class SubmitApprenticeshipInformationModel : INetworkHubLink
{
    public string? EmployerName { get; set; }
    public string? EmployerAddress1 { get; set; }
    public string? EmployerAddress2 { get; set; }
    public string? EmployerTownOrCity { get; set; }
    public string? EmployerCounty { get; set; }
    public string? EmployerPostcode { get; set; }
    public bool ShowApprenticeshipInformation { get; set; }
    public string? NetworkHubLink { get; set; }
}
