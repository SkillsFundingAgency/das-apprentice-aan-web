using SFA.DAS.Aan.SharedUi.Constants;

namespace SFA.DAS.Aan.SharedUi.OuterApi.Responses;

public class MemberProfile
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? OrganisationName { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? RegionId { get; set; }
    public string RegionName { get; set; } = string.Empty;
    public Role UserType { get; set; }
    public bool? IsRegionalChair { get; set; }
    public Apprenticeship Apprenticeship { get; set; } = null!;
    public IEnumerable<Profile> Profiles { get; set; } = Enumerable.Empty<Profile>();
}
