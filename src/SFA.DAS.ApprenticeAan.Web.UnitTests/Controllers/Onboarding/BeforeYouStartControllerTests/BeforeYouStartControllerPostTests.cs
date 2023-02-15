using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
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
        sut.Post();

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => !m.HasAcceptedTermsAndConditions)));
    }
}
