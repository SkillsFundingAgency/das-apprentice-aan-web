using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface IProfileService
{
    Task<List<Profile>> GetProfilesByUserType(string userType);
}