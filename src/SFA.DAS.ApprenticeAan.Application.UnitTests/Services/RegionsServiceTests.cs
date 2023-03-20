using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Services;

[TestFixture]
public class RegionsServiceTests
{
    [MoqAutoData]
    public async Task Post_SetsSelectedRegionInOnBoardingSessionModel(
      Mock<IOuterApiClient> outerApiClient)
    {
        var sut = new RegionsService(outerApiClient.Object);

        var result = await sut.GetRegions();
        result.Should().NotBeNullOrEmpty();
        result.Should().BeInAscendingOrder(x => x.Ordering);
    }
}