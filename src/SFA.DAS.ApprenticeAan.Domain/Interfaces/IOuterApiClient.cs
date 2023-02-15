using RestEase;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces
{
    public interface IOuterApiClient
    {
        [Get("/regions")]
        Task<GetRegionsResult> GetRegions();
    }
}