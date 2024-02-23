using System.Net;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Application.Services;


public class ApprenticeAccountService : IApprenticeAccountService
{
    private readonly IOuterApiClient _client;
    public ApprenticeAccountService(IOuterApiClient client)
    {
        _client = client;
    }

    public async Task<ApprenticeAccount?> GetApprenticeAccountDetails(Guid apprenticeId)
    {
        var response = await _client.GetApprenticeAccount(apprenticeId, CancellationToken.None);
        return response.ResponseMessage.StatusCode switch
        {
            HttpStatusCode.NotFound => null,
            HttpStatusCode.OK => response.GetContent(),
            _ => throw new InvalidOperationException($"Outer api came back with unsuccessful response. StatusCode:{response.ResponseMessage.StatusCode}")
        };
    }
}
