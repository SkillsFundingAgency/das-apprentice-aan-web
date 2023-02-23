using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.BeforeYouStartControllerTests;

[TestFixture]
public class BeforeYouStartControllerPostTests
{
    [MoqAutoData]
    public void Post_SetsEmptyOnBoardingSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] BeforeYouStartController sut)
    {
        var result = sut.Post();

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => !m.HasAcceptedTermsAndConditions)));
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.TermsAndConditions);
    }
}
