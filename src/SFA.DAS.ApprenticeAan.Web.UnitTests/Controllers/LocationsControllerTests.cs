using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class LocationsControllerTests
{
    [Test]
    [MoqAutoData]
    public void GetAddresses_ReturnsApiResponse(
        [Frozen] Mock<IOuterApiClient> outerAPiMock,
        [Greedy] LocationsController sut,
        GetAddressesResult expectedResult,
        string query)
    {
        outerAPiMock.Setup(o => o.GetAddresses(query)).ReturnsAsync(expectedResult);

        var actualResult = sut.GetAddresses(query);

        actualResult.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedResult.Addresses);
    }

    [Test, MoqAutoData]
    public void GetLocationsBySearch_ReturnsApiResponse(
       [Frozen] Mock<IOuterApiClient> outerAPiMock,
       [Greedy] LocationsController sut,
       GetLocationsBySearchApiResponse expectedResult,
       string query)
    {
        outerAPiMock.Setup(o => o.GetLocationsBySearch(query, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);
        var actualResult = sut.GetLocationsBySearch(query, new CancellationToken());
        actualResult.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedResult.Locations);
    }
}
