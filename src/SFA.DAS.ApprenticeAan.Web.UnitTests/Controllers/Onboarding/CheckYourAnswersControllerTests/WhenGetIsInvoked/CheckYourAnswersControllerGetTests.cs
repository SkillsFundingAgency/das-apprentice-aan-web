using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public void Get_HasSeenPreview_DoesNotRequestMyApprenticeshipData(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CheckYourAnswersController sut,
        OnboardingSessionModel onboardingSessionModel,
        Guid apprenticeId)
    {
        //Arrange
        onboardingSessionModel.ProfileData = GetProfileData();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(onboardingSessionModel);

        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new() { HttpContext = new DefaultHttpContext() { User = user.HttpContext!.User } };

        sut.AddUrlHelperMock();

        //Action
        var result = sut.Get();

        //Assert
        result.As<ViewResult>().ViewName.Should().Be(CheckYourAnswersController.ViewPath);
    }
}
