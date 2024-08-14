using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.Services;

public class ApprenticeAccountProvider : IApprenticeAccountProvider
{
    private readonly IOuterApiClient _client;

    public ApprenticeAccountProvider(IOuterApiClient client)
    {
        _client = client;
    }

    public async Task<IApprenticeAccount?> GetApprenticeAccount(Guid id)
    {
        var apprentice = await _client.GetApprentice(id);
        return apprentice.GetContent();
    }

    public async Task<IApprenticeAccount?> PutApprenticeAccount(string email, string govUkIdentifier)
    {
        return await _client.PutApprentice(new PutApprenticeRequest
        {
            Email = email,
            GovUkIdentifier = govUkIdentifier
        });
    }
}