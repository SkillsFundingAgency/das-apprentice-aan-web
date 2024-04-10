using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces
{
    public interface IMemberService
    {
        Task UpdateMemberDetails(Guid apprenticeId, string firstName, string lastName, CancellationToken cancellationToken);
    }
}
