using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

public sealed class ApprenticeAccount : IApprenticeAccount
{
    public Guid ApprenticeId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public bool TermsOfUseAccepted { get; set; }
}
