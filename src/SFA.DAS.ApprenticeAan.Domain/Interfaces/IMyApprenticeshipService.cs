using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface IMyApprenticeshipService
{
    Task<MyApprenticeship> TryCreateMyApprenticeship(Guid apprenticeId, string lastName, string email, DateTime dateOfBirth, CancellationToken cancellationToken);
}
