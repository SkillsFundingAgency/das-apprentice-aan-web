namespace SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;
public class EditAreaOfInterestViewModel : SubmitAreaOfInterestModel, INetworkHubLink
{
    public string? NetworkHubLink { get; set; }
    public string FirstSectionTitle { get; set; } = null!;
    public string SecondSectionTitle { get; set; } = null!;
    public string YourAmbassadorProfileUrl { get; set; } = null!;
}

