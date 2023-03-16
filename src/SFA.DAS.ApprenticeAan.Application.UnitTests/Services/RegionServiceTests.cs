using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Services;

[TestFixture]
public class RegionServiceTests
{
    [MoqAutoData]
    public async Task Post_SetsSelectedRegionInOnBoardingSessionModel(
      Mock<IOuterApiClient> outerApiClient)
    {
        var regionService = new RegionService(outerApiClient.Object);

        var result = await regionService.GetRegions();
        result.Should().NotBeNullOrEmpty();
    }
}
