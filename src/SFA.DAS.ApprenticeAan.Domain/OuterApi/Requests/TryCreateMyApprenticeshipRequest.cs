using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;

public class TryCreateMyApprenticeshipRequest
{
    public Guid ApprenticeId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }

    public static implicit operator TryCreateMyApprenticeshipRequest(ApprenticeAccount source) => new()
    {
        ApprenticeId = source.ApprenticeId,
        FirstName = source.FirstName,
        LastName = source.LastName,
        Email = source.Email,
        DateOfBirth = source.DateOfBirth
    };
}
