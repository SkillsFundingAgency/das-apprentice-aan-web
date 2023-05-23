using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CheckYourAnswersControllerTests;

public class CheckYourAnswersControllerPostTests : CheckYourAnswersControllerTestsBase
{
    [Test, MoqAutoData]
    public async Task Post_CallsOuterApiToCreateApprenticeMemberAndNavigatesToApplicationSubmitted(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IOuterApiClient> outerApiClientMock,
        [Greedy] CheckYourAnswersController sut,
        OnboardingSessionModel onboardingSessionModel,
        Guid apprenticeId)
    {
        onboardingSessionModel.ProfileData = GetProfileData();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(onboardingSessionModel);
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);

        var authServiceMock = new Mock<IAuthenticationService>();
        authServiceMock
            .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>())).Returns(Task.CompletedTask);

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(_ => _.GetService(typeof(IAuthenticationService)))
            .Returns(authServiceMock.Object);

        serviceProviderMock
            .Setup(_ => _.GetService(typeof(ITempDataDictionaryFactory)))
            .Returns(new Mock<ITempDataDictionaryFactory>().Object);

        sut.ControllerContext = new() { HttpContext = new DefaultHttpContext() { User = user, RequestServices = serviceProviderMock.Object } };

        var result = await sut.Post();

        outerApiClientMock.Verify(o => o.PostApprenticeMember(It.Is<CreateApprenticeMemberRequest>(r => r.OrganisationName == onboardingSessionModel.GetProfileValue(ProfileDataId.EmployerName))));
        result.As<ViewResult>().ViewName.Should().Be(CheckYourAnswersController.ApplicationSubmittedViewPath);
    }
}
