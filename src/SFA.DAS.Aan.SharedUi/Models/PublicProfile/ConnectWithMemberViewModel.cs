namespace SFA.DAS.Aan.SharedUi.Models.PublicProfile;

public class ConnectWithMemberViewModel : ConnectWithMemberSubmitModel
{
    public Guid MemberId { get; set; }
    public string ControllerName { get; set; } = "MemberProfile";
    public string ActionName { get; set; } = "Post";
    public bool IsConnectWithMemberVisible { get; set; }
    public string FirstName { get; set; } = null!;
    public bool IsLoggedInUserMemberProfile { get; set; }

}
