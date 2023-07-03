using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestEase;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Extensions;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class NetworkEventsControllerTests
{
    private static readonly string AllNetworksUrl = Guid.NewGuid().ToString();


    [Test, MoqAutoData]
    public void GetCalendarEvents_ReturnsApiResponse(
    [Frozen] Mock<IOuterApiClient> outerApiMock,
    [Greedy] NetworkEventsController sut,
    GetCalendarEventsQueryResult expectedResult,
    DateTime? fromDate,
    DateTime? toDate,
    Guid apprenticeId)
    {
        var eventFormats = new List<EventFormat>
        {
            EventFormat.InPerson,
            EventFormat.Online,
            EventFormat.Hybrid
        };
        var eventTypes = new List<int>();
        var regions = new List<int>();
        var fromDateFormatted = fromDate?.ToString("yyyy-MM-dd")!;
        var toDateFormatted = toDate?.ToString("yyyy-MM-dd")!;
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        outerApiMock.Setup(o => o.GetCalendarEvents(It.IsAny<Guid>(), fromDateFormatted, toDateFormatted, eventFormats, eventTypes, regions, It.IsAny<CancellationToken>())).ReturnsAsync(expectedResult);

        var request = new GetNetworkEventsRequest
        {
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = eventTypes,
            RegionId = regions
        };

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.NetworkEvents, AllNetworksUrl);

        var actualResult = sut.Index(request, new CancellationToken());
        var expectedEventFormatChecklistLookup = new ChecklistLookup[]
        {
            new(EventFormat.InPerson.GetDescription()!, EventFormat.InPerson.ToString(),
                request.EventFormat.Exists(x => x == EventFormat.InPerson)),
            new(EventFormat.Online.GetDescription()!, EventFormat.Online.ToString(),
                request.EventFormat.Exists(x => x == EventFormat.Online)),
            new(EventFormat.Hybrid.GetDescription()!, EventFormat.Hybrid.ToString(),
                request.EventFormat.Exists(x => x == EventFormat.Hybrid))
        };

        var viewResult = actualResult.Result.As<ViewResult>();
        var model = viewResult.Model as NetworkEventsViewModel;
        model!.Pagination.Page.Should().Be(expectedResult.Page);
        model!.Pagination.PageSize.Should().Be(expectedResult.PageSize);
        model!.Pagination.TotalPages.Should().Be(expectedResult.TotalPages);
        model!.TotalCount.Should().Be(expectedResult.TotalCount);
        model.FilterChoices.FromDate?.ToApiString().Should().Be(fromDateFormatted);
        model.FilterChoices.ToDate?.ToApiString().Should().Be(toDateFormatted);
        model.FilterChoices.EventFormatChecklistDetails.Lookups.Should().BeEquivalentTo(expectedEventFormatChecklistLookup);

        outerApiMock.Verify(o => o.GetCalendarEvents(It.IsAny<Guid>(), fromDateFormatted, toDateFormatted, eventFormats, eventTypes, regions, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    [MoqAutoData]
    public void Details_ReturnsEventDetailsViewModel(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventsController sut,
        CalendarEvent calendarEvent,
        Guid apprenticeId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        var response = new Response<CalendarEvent>(string.Empty, new(HttpStatusCode.OK), () => calendarEvent);
        outerApiMock.Setup(o => o.GetCalendarEventDetails(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

        var result = (ViewResult)sut.Details(apprenticeId, new CancellationToken()).Result;

        Assert.That(result.Model, Is.TypeOf<NetworkEventDetailsViewModel>());
    }

    [Test]
    [MoqAutoData]
    public void Details_InvokesOuterApiClientGetEventDetails(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventsController sut,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var result = sut.Details(apprenticeId, cancellationToken);

        outerApiMock.Verify(o => o.GetCalendarEventDetails(apprenticeId, It.IsAny<Guid>(), cancellationToken), Times.Once());
    }

    [Test]
    [MoqAutoData]
    public void Details_CalendarEventIdIsNotFound_ThrowsInvalidOperationException(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventsController sut,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        var calendarEvent = new CalendarEvent() { CalendarEventId = Guid.Empty };
        var response = new Response<CalendarEvent>(string.Empty, new(HttpStatusCode.NotFound), () => null!);
        outerApiMock.Setup(o => o.GetCalendarEventDetails(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        Assert.That(() => sut.Details(apprenticeId, cancellationToken), Throws.InvalidOperationException);
    }

    [Test]
    [MoqAutoData]
    public void SignUpConfirmation_ReturnsSignUpConfirmationView(
        [Greedy] NetworkEventsController sut,
        Guid apprenticeId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var result = sut.SignUpConfirmation();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.ViewName, Is.EqualTo(NetworkEventsController.SignUpConfirmationViewPath));
        });
    }

    [Test]
    [MoqAutoData]
    public void CancellationConfirmation_ReturnsCancellationConfirmationView(
        [Greedy] NetworkEventsController sut,
        Guid apprenticeId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var result = sut.CancellationConfirmation();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.ViewName, Is.EqualTo(NetworkEventsController.CancellationConfirmationViewPath));
        });
    }

    [Test]
    [MoqAutoData]
    public async Task SetAttendanceStatus_InvokesOuterApiClientPutAttendance(
        ApplicationConfiguration config,
        Mock<IOuterApiClient> outerApiMock,
        Guid apprenticeId,
        Guid calendarEventId,
        bool newStatus)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);

        var sut = new NetworkEventsController(outerApiMock.Object, config);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        await sut.SetAttendanceStatus(calendarEventId, newStatus);

        outerApiMock.Verify(o => o.PutAttendance(calendarEventId,
                                                 It.IsAny<Guid>(),
                                                 It.Is<SetAttendanceStatusRequest>(a => a.IsAttending == newStatus),
                                                 It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    [MoqAutoData]
    public async Task SetAttendanceStatus_NewStatusIsTrue_RedirectsToSignUpConfirmation(
        [Greedy] NetworkEventsController sut,
        Guid apprenticeId,
        Guid calendarEventId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var result = await sut.SetAttendanceStatus(calendarEventId, true);

        Assert.That(result.As<RedirectToActionResult>().ActionName, Is.EqualTo("SignUpConfirmation"));
    }

    [Test]
    [MoqAutoData]
    public async Task SetAttendanceStatus_NewStatusIsFalse_RedirectsToCancellationConfirmation(
        [Greedy] NetworkEventsController sut,
        Guid apprenticeId,
        Guid calendarEventId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var result = await sut.SetAttendanceStatus(calendarEventId, false);

        Assert.That(result.As<RedirectToActionResult>().ActionName, Is.EqualTo("CancellationConfirmation"));
    }
}
