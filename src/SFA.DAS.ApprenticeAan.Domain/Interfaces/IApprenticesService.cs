using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Domain.Interfaces;

public interface IApprenticesService
{
    Task<Apprentice?> GetApprentice(Guid apprenticeId);
}
