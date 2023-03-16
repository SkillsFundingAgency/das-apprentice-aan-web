using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.BeforeYouStartControllerTests;

[TestFixture]
public class BeforeYouStartControllerPostTests
{
    [MoqAutoData]
    public async Task Post_SetsEmptyOnBoardingSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] BeforeYouStartController sut)
    {
        var result = await sut.Post();

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => !m.HasAcceptedTermsAndConditions)));
        result.As<RedirectToRouteResult>().RouteName.Should().Be("TermsAndConditions");
    }

    [MoqAutoData]
    public async Task Post_GetProfiles_UpdatesSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IProfileService> profileServiceMock,
        [Greedy] BeforeYouStartController sut,
        List<Profile> profiles)
    {
        const string userType = "apprentice";

        OnboardingSessionModel sessionModel = new();

        profileServiceMock.Setup(s => s.GetProfilesByUserType(userType)).ReturnsAsync(profiles);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionServiceMock.Object.Set(sessionModel);

        await sut.Post();

        sessionServiceMock.Verify(s => s.Set(sessionModel));

        sessionModel.ProfileData.Should().BeEquivalentTo(profiles);
    }

    [MoqAutoData]
    public async Task Post_SessionModel_RedirectsRouteToTermsAndConditions(
         [Frozen] Mock<ISessionService> sessionServiceMock,
         [Greedy] BeforeYouStartController sut)
    {
        OnboardingSessionModel sessionModel = new();

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = await sut.Post();

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.TermsAndConditions);
    }
}