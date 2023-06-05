namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class NetworkEventsControllerTests
{
    [Test, MoqAutoData]
    public void GetCalendarEvents_ReturnsApiResponse(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventsController sut,
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
