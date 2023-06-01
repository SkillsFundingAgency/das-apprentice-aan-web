using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Models.CalendarEvents;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;
public class CalendarEventsControllerTests
{
    [Test]
    [MoqAutoData]
    public void Index_ReturnsEventDetailsViewModel(
        [Greedy] CalendarEventsController sut,
        Guid eventId)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(eventId);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var result = (ViewResult)sut.Index(eventId, new CancellationToken()).Result;

        Assert.That(result.Model, Is.TypeOf<EventDetailsViewModel>());
    }

    [Test]
    [MoqAutoData]
    public void Index_InvokesOuterApiClientGetEventDetails(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] CalendarEventsController sut,
        Guid eventId,
        CancellationToken cancellationToken)
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(eventId);

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        var result = sut.Index(eventId, cancellationToken);

        outerApiMock.Verify(o => o.GetEventDetails(eventId, It.IsAny<Guid>(), cancellationToken), Times.Once());
    }
}
