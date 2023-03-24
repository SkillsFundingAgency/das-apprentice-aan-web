using System.Net;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Web.Services;

public class ApprenticeAccountService : IApprenticeAccountService
{
    private readonly IOuterApiClient _client;
    public ApprenticeAccountService(IOuterApiClient client)
    {
        _client = client;
    }

    public async Task<ApprenticeAccount?> GetApprenticeAccountDetails(Guid apprenticeId)
    {
        try
        {
            return await _client.GetApprenticeAccount(apprenticeId);
        }
        catch (RestEase.ApiException ex)
        {
            //User has not completed registration, this will navigate to accounts web
            if (ex.StatusCode == HttpStatusCode.NotFound) return null;
            throw;
        }
    }
}
