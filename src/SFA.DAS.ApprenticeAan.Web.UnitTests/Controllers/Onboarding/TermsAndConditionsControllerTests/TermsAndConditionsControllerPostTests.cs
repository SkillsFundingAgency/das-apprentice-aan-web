using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
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
    }
}
