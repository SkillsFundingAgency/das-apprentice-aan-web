namespace SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;
public class EditAreaOfInterestViewModel : SubmitAreaOfInterestModel, INetworkHubLink
{
    public string? NetworkHubLink { get; set; }
    public string YourAmbassadorProfileUrl { get; set; } = null!;
}

