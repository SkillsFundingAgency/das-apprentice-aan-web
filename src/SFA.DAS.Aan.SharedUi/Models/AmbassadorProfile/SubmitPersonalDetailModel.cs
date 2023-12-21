using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;

namespace SFA.DAS.Aan.SharedUi.Models;

public class SubmitPersonalDetailModel : INetworkHubLink
{
    public int RegionId { get; set; }
    public string? JobTitle { get; set; }
    public bool ShowJobTitle { get; set; }
    public string? Biography { get; set; }
    public bool ShowBiography { get; set; }
    public string OrganisationName { get; set; } = null!;
    public MemberUserType UserType { get; set; }
    public string? NetworkHubLink { get; set; }
}
