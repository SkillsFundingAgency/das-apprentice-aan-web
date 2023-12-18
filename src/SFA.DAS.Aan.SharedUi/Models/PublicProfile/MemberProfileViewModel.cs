namespace SFA.DAS.Aan.SharedUi.Models.PublicProfile;

public class MemberProfileViewModel : ConnectWithMemberSubmitModel, INetworkHubLink
{
    public Guid MemberId { get; set; }
    public string ControllerName { get; set; } = "MemberProfile";
    public string ActionName { get; set; } = "Post";
    public bool IsConnectWithMemberVisible { get; set; }
    public bool IsLoggedInUserMemberProfile { get; set; }
    public string FirstName { get; set; } = null!;
    public ConnectViaLinkedInViewModel ConnectViaLinkedIn { get; set; } = new();
    public MemberInformationSectionViewModel MemberInformation { get; set; } = new();
    public ApprenticeshipSectionViewModel ApprenticeshipInformation { get; set; } = new();
    public AreasOfInterestSectionViewModel AreasOfInterest { get; set; } = new();
    public string? NetworkHubLink { get; set; }
}
