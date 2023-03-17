using RestEase;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface IOuterApiClient
{
    [Get("/regions")]
    Task<GetRegionsResult> GetRegions();

    [Get("/profiles/{userType}")]
    Task<GetProfilesResult> GetProfilesByUserType([Path("userType")] string userType);
    
    [Get("/apprentices/account/{apprenticeId}")]
    Task<ApprenticeAccount?> GetApprenticeAccount([Path] Guid apprenticeId);
}