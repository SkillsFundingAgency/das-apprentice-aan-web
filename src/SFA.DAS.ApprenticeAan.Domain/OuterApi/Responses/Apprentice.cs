using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

public class Apprentice : IApprenticeAccount
{
    public Guid MemberId { get; set; }
    public Guid ApprenticeId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool TermsOfUseAccepted { get; set; }
    public string Email { get; set; } = null!;
    public string OrganisationName { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int RegionId { get; set; }
}
