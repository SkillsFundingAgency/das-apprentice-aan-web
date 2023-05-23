using System.Net;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Application.Services;

public class ApprenticeService : IApprenticeService
{
    private readonly IOuterApiClient _client;

    public ApprenticeService(IOuterApiClient client)
    {
        _client = client;
    }

    public async Task<Apprentice?> GetApprentice(Guid apprenticeId)
    {
        var response = await _client.GetApprentice(apprenticeId);
        return response.ResponseMessage.StatusCode switch
        {
            HttpStatusCode.NotFound => null,
            HttpStatusCode.OK => response.GetContent(),
            _ => throw new InvalidOperationException($"Outer api came back with unsuccessful response. StatusCode:{response.ResponseMessage.StatusCode}")
        };
    }
}
