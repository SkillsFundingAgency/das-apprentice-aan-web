using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;
public class SearchNetworkEventsControllerTests
{
    [Test, MoqAutoData]
    public void GetCalendarEvents_ReturnsApiResponse(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] SearchNetworkEventsController sut,
        GetCalendarEventsQueryResult expectedResult,
        Guid apprenticeId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        outerApiMock.Setup(o => o.GetCalendarEvents(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var actualResult = sut.Index(new CancellationToken());

        var viewResult = actualResult.Result.As<ViewResult>();
        viewResult.Model.Should().BeEquivalentTo(expectedResult);
    }
}
