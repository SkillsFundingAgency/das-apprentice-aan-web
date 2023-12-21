namespace SFA.DAS.Aan.SharedUi.Models.EditContactDetail;
public class SubmitContactDetailModel : INetworkHubLink
{
    public string? LinkedinUrl { get; set; }
    public bool ShowLinkedinUrl { get; set; }
    public string? NetworkHubLink { get; set; }
}
