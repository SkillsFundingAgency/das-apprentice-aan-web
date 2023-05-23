using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface IApprenticeService
{
    Task<Apprentice?> GetApprentice(Guid apprenticeId);
    Task<CreateApprenticeMemberResponse> PostApprenticeship(CreateApprenticeMemberRequest request);
}
