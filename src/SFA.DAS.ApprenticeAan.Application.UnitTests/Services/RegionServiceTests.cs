using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Services;

[TestFixture]
public class RegionServiceTests
{
    [MoqAutoData]
    public async Task Post_SetsSelectedRegionInOnBoardingSessionModel(
    Mock<IRegionService> regionServiceMock, Task<List<Region>> regions)
    {
        regionServiceMock.Setup(x => x.GetRegions()).Returns(regions);

        var result = await regionServiceMock.Object.GetRegions();
        result.Should().NotBeNullOrEmpty();
    }
}
