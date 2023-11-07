using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;

namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
public class GetMemberProfileResponse
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? OrganisationName { get; set; }
    public int? RegionId { get; set; }
    public string RegionName { get; set; } = null!;
    public MemberUserType UserType { get; set; }
    public bool IsRegionalChair { get; set; }
    public ApprenticeshipDetails? Apprenticeship { get; set; }
    public IEnumerable<MemberProfile> Profiles { get; set; } = null!;
    public IEnumerable<MemberPreference> Preferences { get; set; } = null!;
}