using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestEase;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class NetworkEventsControllerTests
{
    [Test, MoqAutoData]
    public void GetCalendarEvents_ReturnsApiResponse(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventsController sut,
        GetCalendarEventsQueryResult expectedResult,
        DateTime? fromDate,
        DateTime? toDate,
        Guid apprenticeId)
    {
        var eventFormats = new List<EventFormat>();
        var fromDateFormatted = fromDate?.ToString("yyyy-MM-dd")!;
        var toDateFormatted = toDate?.ToString("yyyy-MM-dd")!;
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        outerApiMock.Setup(o => o.GetCalendarEvents(It.IsAny<Guid>(), fromDateFormatted, toDateFormatted, eventFormats, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        var request = new GetNetworkEventsRequest
        {
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats
        };

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var actualResult = sut.Index(request, new CancellationToken());

        var viewResult = actualResult.Result.As<ViewResult>();
        viewResult.Model.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    [MoqAutoData]
    public void Details_ReturnsEventDetailsViewModel(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventsController sut,
        CalendarEvent calendarEvent,
        Guid eventId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(eventId);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        var response = new Response<CalendarEvent>(string.Empty, new(HttpStatusCode.OK), () => calendarEvent);
        outerApiMock.Setup(o => o.GetCalendarEventDetails(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

        var result = (ViewResult)sut.Details(eventId, new CancellationToken()).Result;

        Assert.That(result.Model, Is.TypeOf<NetworkEventDetailsViewModel>());
    }

    [Test]
    [MoqAutoData]
    public void Details_InvokesOuterApiClientGetEventDetails(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventsController sut,
        Guid eventId,
        CancellationToken cancellationToken)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(eventId);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var result = sut.Details(eventId, cancellationToken);

        outerApiMock.Verify(o => o.GetCalendarEventDetails(eventId, It.IsAny<Guid>(), cancellationToken), Times.Once());
    }

    [Test]
    [MoqAutoData]
    public void Details_CalendarEventIdIsNotFound_ThrowsInvalidOperationException(
     [Frozen] Mock<IOuterApiClient> outerApiMock,
     [Greedy] NetworkEventsController sut,
     Guid eventId,
     CancellationToken cancellationToken)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(eventId);
        var calendarEvent = new CalendarEvent() { CalendarEventId = Guid.Empty };
        var response = new Response<CalendarEvent>(string.Empty, new(HttpStatusCode.NotFound), () => null!);
        outerApiMock.Setup(o => o.GetCalendarEventDetails(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        Assert.That(() => sut.Details(eventId, cancellationToken), Throws.InvalidOperationException);
    }
}
