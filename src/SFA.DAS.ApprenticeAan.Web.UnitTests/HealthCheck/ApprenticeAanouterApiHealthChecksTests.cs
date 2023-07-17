using AutoFixture.NUnit3;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.HealthCheck;
using SFA.DAS.Testing.AutoFixture;
using static Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus;
using Calendar = SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses.Calendar;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.HealthCheck;
public class ApprenticeAanouterApiHealthChecksTests
{
    [Test, MoqAutoData]
    public async Task CheckHealthAsync_ValidQueryResult_ReturnsHealthyStatus(
           [Frozen] Mock<IOuterApiClient> apiClient,
           HealthCheckContext healthCheckContext,
           List<Calendar> calendars,
           ApprenticeAanOuterApiHealthCheck healthCheck)
    {
        apiClient.Setup(x => x.GetCalendars()).ReturnsAsync(calendars);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        Assert.That(actual.Status, Is.EqualTo(Healthy));
    }

    [Test, MoqAutoData]
    public async Task CheckHealthAsync_NotValidQueryResult_ReturnsUnHealthyStatus(
        [Frozen] Mock<IOuterApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        List<Calendar> calendars,
        ApprenticeAanOuterApiHealthCheck healthCheck)
    {
        apiClient.Setup(x => x.GetCalendars()).ReturnsAsync(calendars);

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        Assert.That(actual.Status, Is.EqualTo(Healthy));
    }

    [Test, MoqAutoData]
    public async Task CheckHealthAsync_ExceptionThrown_ReturnsUnHealthyStatus(
        [Frozen] Mock<IOuterApiClient> apiClient,
        HealthCheckContext healthCheckContext,
        ApprenticeAanOuterApiHealthCheck healthCheck)
    {
        apiClient.Setup(x => x.GetCalendars()).ThrowsAsync(new InvalidOperationException());

        var actual = await healthCheck.CheckHealthAsync(healthCheckContext, CancellationToken.None);

        Assert.That(actual.Status, Is.EqualTo(Unhealthy));
    }
}
