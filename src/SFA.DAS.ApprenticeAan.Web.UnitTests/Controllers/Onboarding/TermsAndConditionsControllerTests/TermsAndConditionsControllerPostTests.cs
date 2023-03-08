using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.TermsAndConditionsControllerTests
{
    [TestFixture]
    public class TermsAndConditionsControllerPostTests
    {
        [MoqAutoData]
        public void Post_SetsEmptyOnBoardingSessionModel(
       [Frozen] Mock<ISessionService> sessionServiceMock,
       [Greedy] TermsAndConditionsController sut)
        {
            sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.LineManager);

            OnboardingSessionModel value = new();
            sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(value);

            sut.Post();

            sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.Equals(value) && m.HasAcceptedTermsAndConditions)));
        }

        [MoqAutoData]
        public void Post_RedirectToRouteResult_RedirectsToLineManager(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] TermsAndConditionsController sut)
        {
            sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.LineManager);

            OnboardingSessionModel value = new();
            sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(value);

            var result = sut.Post();

            sessionServiceMock.Verify(s => s.Set(value));

            result.As<RedirectToRouteResult>().RouteName.Should().Be("LineManager");
        }
    }
}
