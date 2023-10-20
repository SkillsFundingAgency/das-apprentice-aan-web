using Microsoft.Extensions.Logging;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;

namespace SFA.DAS.ApprenticeAan.Application.Services;

public class MyApprenticeshipService : IMyApprenticeshipService
{
    private readonly IOuterApiClient _outerApiClient;
    private readonly ILogger<MyApprenticeshipService> _logger;

    public MyApprenticeshipService(IOuterApiClient outerApiClient, ILogger<MyApprenticeshipService> logger)
    {
        _outerApiClient = outerApiClient;
        _logger = logger;
    }

    public async Task<MyApprenticeship> TryCreateMyApprenticeship(Guid apprenticeId, string lastName, string email, DateTime dateOfBirth, CancellationToken cancellationToken)
    {
        var response = await _outerApiClient.GetMyApprenticeship(apprenticeId, cancellationToken);
        if (response.ResponseMessage.IsSuccessStatusCode)
        {
            _logger.LogInformation("MyApprenticeship record already exists for apprenticeId: {apprenticeId}", apprenticeId);
            return response.GetContent();
        }

        _logger.LogInformation("Creating MyApprenticeship record for apprenticeId: {apprenticeId}", apprenticeId);
        // The stage apprentice is being checked in the auth filter, so expect it to be always there
        var stagedApprenticeResponse = await _outerApiClient.GetStagedApprentice(new GetStagedApprenticeRequest(lastName, dateOfBirth.ToString("yyyy-MM-dd"), email), cancellationToken);
        CreateMyApprenticeshipRequest request = stagedApprenticeResponse.GetContent()!;
        request.ApprenticeId = apprenticeId;
        await _outerApiClient.CreateMyApprenticeship(request, cancellationToken);

        response = await _outerApiClient.GetMyApprenticeship(apprenticeId, cancellationToken);
        return response.GetContent();
    }
}
