using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;

namespace SFA.DAS.ApprenticeAan.Web.HealthCheck;

public class ApprenticeAanOuterApiHealthCheck : IHealthCheck
{
    public const string HealthCheckResultDescription = "Apprentice Aan Outer API Health Check";

    private readonly IOuterApiClient _outerApiClient;
    private readonly ILogger<ApprenticeAanOuterApiHealthCheck> _logger;

    public ApprenticeAanOuterApiHealthCheck(ILogger<ApprenticeAanOuterApiHealthCheck> logger, IOuterApiClient outerApiClient)
    {
        _logger = logger;
        _outerApiClient = outerApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Apprentice Aan Outer API pinging call");
        try
        {
            await _outerApiClient.GetCalendars();
            return HealthCheckResult.Healthy(HealthCheckResultDescription);

        }
        catch (Exception)
        {
            _logger.LogError("Apprentice Aan Outer API ping failed");
            return HealthCheckResult.Unhealthy(HealthCheckResultDescription);
        }
    }
}
