using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Http;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CheckYourAnswersControllerTests.WhenGetIsInvoked;
public class CheckYourAnswersControllerGetTests : CheckYourAnswersControllerTestsBase
{
    [Test, MoqAutoData]
    public async Task Get_HasSeenPreview_DoesNotRequestMyApprenticeshipData(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IOuterApiClient> apiClientMock,
        [Greedy] CheckYourAnswersController sut,
        OnboardingSessionModel onboardingSessionModel,
        Guid apprenticeId)
    {
        //Arrange
        onboardingSessionModel.ProfileData = GetProfileData();
        onboardingSessionModel.HasSeenPreview = true;
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(onboardingSessionModel);

        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new() { HttpContext = new DefaultHttpContext() { User = user } };

        sut.AddUrlHelperMock();

        //Action
        await sut.Get();

        apiClientMock.Verify(c => c.GetMyApprenticeship(It.IsAny<Guid>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Get_HasNotSeenPreview_RequestsMyApprenticeshipData(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IOuterApiClient> apiClientMock,
        [Greedy] CheckYourAnswersController sut,
        OnboardingSessionModel onboardingSessionModel,
        Guid apprenticeId)
    {
        //Arrange
        onboardingSessionModel.ProfileData = GetProfileData();
        onboardingSessionModel.HasSeenPreview = false;
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(onboardingSessionModel);

        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new() { HttpContext = new DefaultHttpContext() { User = user } };

        sut.AddUrlHelperMock();

        //Action
        await sut.Get();

        apiClientMock.Verify(c => c.GetMyApprenticeship(apprenticeId));
    }
}
