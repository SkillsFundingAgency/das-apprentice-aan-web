namespace SFA.DAS.Aan.SharedUi.Models.EditContactDetail;
public class EditContactDetailViewModel : SubmitContactDetailModel, INetworkHubLink
{
    public string? NetworkHubLink { get; set; }
    public string Email { get; set; } = null!;
    public string YourAmbassadorProfileUrl { get; set; } = null!;
}
