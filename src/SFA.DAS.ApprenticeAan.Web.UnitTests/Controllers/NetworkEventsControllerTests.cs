using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;
public class NetworkEventsControllerTests
{
    [Test, MoqAutoData]
    public void GetCalendarEvents_ReturnsApiResponse(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventsController sut,
        GetCalendarEventsQueryResult expectedResult,
        DateTime? startDate,
        DateTime? endDate,
        Guid apprenticeId)
    {
        var startDateFormatted = startDate?.ToString("yyyy-MM-dd")!;
        var endDateFormatted = endDate?.ToString("yyyy-MM-dd")!;
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        outerApiMock.Setup(o => o.GetCalendarEvents(It.IsAny<Guid>(), startDateFormatted, endDateFormatted, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        var request = new GetNetworkEventsRequest
        {
            StartDate = startDate,
            EndDate = endDate
        };

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var actualResult = sut.Index(request, new CancellationToken());

        var viewResult = actualResult.Result.As<ViewResult>();
        viewResult.Model.Should().BeEquivalentTo(expectedResult);
    }
}