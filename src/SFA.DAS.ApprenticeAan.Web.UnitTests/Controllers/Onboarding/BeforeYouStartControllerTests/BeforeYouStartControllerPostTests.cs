using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
    public async Task Post_SetsEmptyOnBoardingSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] BeforeYouStartController sut)
    {
        var result = await sut.PostAsync();

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => !m.HasAcceptedTermsAndConditions)));
        result.As<RedirectToRouteResult>().RouteName.Should().Be("TermsAndConditions");
    }

    [MoqAutoData]
    public async Task Post_GetProfiles_UpdatesSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IProfileService> _profileService,
        [Greedy] BeforeYouStartController sut)
    {
        OnboardingSessionModel sessionModel = new();
        var profiles = await _profileService.Object.GetProfiles();

        _profileService.Verify(p => p.GetProfiles());

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionServiceMock.Object.Set(sessionModel);

        await sut.PostAsync();

        sessionServiceMock.Verify(s => s.Set(sessionModel));

        sessionModel.ProfileData.Should().BeEquivalentTo(profiles.ConvertAll(p => new ProfileModel()));
    }

    [MoqAutoData]
    public async Task Post_SessionModel_RedirectsRouteToTermsAndConditions(
         [Frozen] Mock<ISessionService> sessionServiceMock,
         [Greedy] BeforeYouStartController sut)
    {
        OnboardingSessionModel sessionModel = new();

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = await sut.PostAsync();

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be("TermsAndConditions");
    }
}