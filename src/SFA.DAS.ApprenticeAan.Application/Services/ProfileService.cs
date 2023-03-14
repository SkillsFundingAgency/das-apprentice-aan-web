using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IOuterApiClient _outerApiClient;

    public ProfileService(IOuterApiClient outerApiClient)
    {
        _outerApiClient = outerApiClient;
    }

    public async Task<List<Profile>> GetProfilesByUserType(string userType)
    {
        var result = await _outerApiClient.GetProfilesByUserType(userType);
        return result.ProfileModels;
    }
}