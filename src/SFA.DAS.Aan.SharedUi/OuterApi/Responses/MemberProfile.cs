using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.Aan.SharedUi.OuterApi.Responses;

public class MemberProfile
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? OrganisationName { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int? RegionId { get; set; }
    public string RegionName { get; set; } = null!;
    public Role UserType { get; set; }
    public bool? IsRegionalChair { get; set; }
    public Apprenticeship? Apprenticeship { get; set; }
    public IEnumerable<Profile> Profiles { get; set; } = Enumerable.Empty<Profile>();
}
