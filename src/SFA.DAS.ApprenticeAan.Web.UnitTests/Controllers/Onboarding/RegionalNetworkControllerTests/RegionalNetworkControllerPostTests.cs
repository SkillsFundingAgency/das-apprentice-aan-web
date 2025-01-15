using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.RegionalNetworkControllerTests;

[TestFixture]
public class RegionalNetworkControllerPostTests
{
    [MoqAutoData]
    public void Post_WhenHasSeenPreview_RedirectsToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] RegionalNetworkController sut,
        RegionalNetworkViewModel submitModel)
    {
        OnboardingSessionModel sessionModel = new() { HasSeenPreview = true };
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Post(submitModel);

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }

    [MoqAutoData]
    public void Post_WhenHasNotSeenPreview_RedirectsToConfirmDetails(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] RegionalNetworkController sut,
        RegionalNetworkViewModel submitModel)
    {
        OnboardingSessionModel sessionModel = new() { HasSeenPreview = false };
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Post(submitModel);

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.ConfirmDetails);
    }
}
