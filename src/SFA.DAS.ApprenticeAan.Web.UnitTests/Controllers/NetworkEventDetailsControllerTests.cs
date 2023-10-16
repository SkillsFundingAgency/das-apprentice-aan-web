using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestEase;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Models.NetworkEvents;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;
public class NetworkEventDetailsControllerTests
{
    private static readonly string AllNetworksUrl = Guid.NewGuid().ToString();
    private static readonly string MemberProfileUrl = Guid.NewGuid().ToString();

    [Test]
    [MoqAutoData]
    public void Details_ReturnsEventDetailsViewModel(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventDetailsController sut,
        CalendarEvent calendarEvent,
        Guid apprenticeId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.NetworkDirectory, AllNetworksUrl).AddUrlForRoute(SharedRouteNames.MemberProfile, MemberProfileUrl);

        var response = new Response<CalendarEvent>(string.Empty, new(HttpStatusCode.OK), () => calendarEvent);
        outerApiMock.Setup(o => o.GetCalendarEventDetails(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

        var result = (ViewResult)sut.Get(apprenticeId, new CancellationToken()).Result;

        Assert.That(result.Model, Is.TypeOf<NetworkEventDetailsViewModel>());
    }

    [Test]
    [MoqAutoData]
    public void Details_InvokesOuterApiClientGetEventDetails(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventDetailsController sut,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var result = sut.Get(apprenticeId, cancellationToken);

        outerApiMock.Verify(o => o.GetCalendarEventDetails(apprenticeId, It.IsAny<Guid>(), cancellationToken), Times.Once());
    }

    [Test]
    [MoqAutoData]
    public void Details_CalendarEventIdIsNotFound_ThrowsInvalidOperationException(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] NetworkEventDetailsController sut,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        var calendarEvent = new CalendarEvent() { CalendarEventId = Guid.Empty };
        var response = new Response<CalendarEvent>(string.Empty, new(HttpStatusCode.NotFound), () => null!);
        outerApiMock.Setup(o => o.GetCalendarEventDetails(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        Assert.That(() => sut.Get(apprenticeId, cancellationToken), Throws.InvalidOperationException);
    }

    [Test]
    [MoqAutoData]
    public void SignUpConfirmation_ReturnsSignUpConfirmationView(
        [Greedy] NetworkEventDetailsController sut,
        Guid apprenticeId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.NetworkEvents, AllNetworksUrl);
        var result = sut.SignUpConfirmation();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.ViewName, Is.EqualTo(NetworkEventDetailsController.SignUpConfirmationViewPath));
        });
    }

    [Test]
    [MoqAutoData]
    public void CancellationConfirmation_ReturnsCancellationConfirmationView(
        [Greedy] NetworkEventDetailsController sut,
        Guid apprenticeId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.NetworkEvents, AllNetworksUrl);
        var result = sut.CancellationConfirmation();

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.ViewName, Is.EqualTo(NetworkEventDetailsController.CancellationConfirmationViewPath));
        });
    }

    [Test]
    [MoqAutoData]
    public async Task SetAttendanceStatus_InvokesOuterApiClientPutAttendance(
        Mock<IOuterApiClient> outerApiMock,
        Guid apprenticeId,
        Guid calendarEventId,
        Mock<IValidator<SubmitAttendanceCommand>> validator,
        bool newStatus)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);

        var sut = new NetworkEventDetailsController(outerApiMock.Object, validator.Object);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        var command = new SubmitAttendanceCommand
        {
            CalendarEventId = calendarEventId,
            NewStatus = newStatus,
            StartDateTime = DateTime.Now.AddDays(1)
        };

        await sut.Post(command, new CancellationToken());

        outerApiMock.Verify(o => o.PutAttendance(calendarEventId,
                                                 It.IsAny<Guid>(),
                                                 It.Is<SetAttendanceStatusRequest>(a => a.IsAttending == newStatus),
                                                 It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test, MoqAutoData]
    public async Task Post_WhenValidationErrorIsRaised(
        Mock<IOuterApiClient> outerApiMock,
        Guid apprenticeId,
        Guid calendarEventId,
        Mock<IValidator<SubmitAttendanceCommand>> validator)
    {

        var calendarEvent = new CalendarEvent { CalendarEventId = calendarEventId };
        var response = new Response<CalendarEvent>(null, new(HttpStatusCode.OK), () => calendarEvent);
        outerApiMock.Setup(o => o.GetCalendarEventDetails(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);

        var sut = new NetworkEventDetailsController(outerApiMock.Object, validator.Object);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        sut.ModelState.AddModelError("key", "message");

        var command = new SubmitAttendanceCommand();

        var result = await sut.Post(command, new CancellationToken()) as ViewResult;

        sut.ModelState.IsValid.Should().BeFalse();
        result!.ViewName.Should().Be(NetworkEventDetailsController.DetailsViewPath);
    }

    [Test]
    [MoqAutoData]
    public async Task SetAttendanceStatus_NewStatusIsTrue_RedirectsToSignUpConfirmation(
        [Greedy] NetworkEventDetailsController sut,
        Guid apprenticeId,
        Guid calendarEventId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var command = new SubmitAttendanceCommand
        {
            CalendarEventId = calendarEventId,
            NewStatus = true,
            StartDateTime = DateTime.Now.AddDays(1)
        };
        var result = await sut.Post(command, new CancellationToken());

        Assert.That(result.As<RedirectToActionResult>().ActionName, Is.EqualTo("SignUpConfirmation"));
    }

    [Test]
    [MoqAutoData]
    public async Task SetAttendanceStatus_NewStatusIsFalse_RedirectsToCancellationConfirmation(
        [Greedy] NetworkEventDetailsController sut,
        Guid apprenticeId,
        Guid calendarEventId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var command = new SubmitAttendanceCommand
        {
            CalendarEventId = calendarEventId,
            NewStatus = false,
            StartDateTime = DateTime.Now.AddDays(1)
        };
        var result = await sut.Post(command, new CancellationToken());

        Assert.That(result.As<RedirectToActionResult>().ActionName, Is.EqualTo("CancellationConfirmation"));
    }
}

