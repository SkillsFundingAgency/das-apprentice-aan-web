using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;

namespace SFA.DAS.Aan.SharedUi.Models;
public class MemberProfileDetail
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? OrganisationName { get; set; }
    public int RegionId { get; set; }
    public string RegionName { get; set; } = null!;
    public MemberUserType UserType { get; set; }
    public bool IsRegionalChair { get; set; }
    public string Sector { get; set; } = null!;
    public string Programmes { get; set; } = null!;
    public string Level { get; set; } = null!;
    public List<string> Sectors { get; set; } = new List<string>();
    public int ActiveApprenticesCount { get; set; }
    public IEnumerable<MemberProfile> Profiles { get; set; } = null!;
}
