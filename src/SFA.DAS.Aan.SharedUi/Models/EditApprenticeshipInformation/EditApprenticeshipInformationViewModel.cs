namespace SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
public class EditApprenticeshipInformationViewModel : SubmitApprenticeshipInformationModel, INetworkHubLink
{
    public string? NetworkHubLink { get; set; }
    public string Sector { get; set; } = null!;
    public string Programmes { get; set; } = null!;
    public string Level { get; set; } = null!;
    public string YourAmbassadorProfileUrl { get; set; } = null!;
}
