using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces
{
    public interface IApprenticeAccountService
    {
        Task<ApprenticeAccount?> GetApprenticeAccountDetails(Guid apprenticeId);
    }
}
