using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.PreviousEngagementControllerTests;

[TestFixture]
public class PreviousEngagementControllerGetTests
{
    [MoqAutoData]
    public void Get_ViewResult_SelectedYes_HasCorrectViewPath(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] PreviousEngagementController sut)
    {
        sut.AddUrlHelperMock();
        OnboardingSessionModel sessionModel = new();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.EngagedWithAPreviousAmbassadorInTheNetwork, Value = "True" });

        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(PreviousEngagementController.ViewPath);
    }

    [MoqAutoData]
    public void Get_ViewResult_SelectedNo_HasCorrectViewPath(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] PreviousEngagementController sut)
    {
        sut.AddUrlHelperMock();
        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.EngagedWithAPreviousAmbassadorInTheNetwork, Value = "False" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(PreviousEngagementController.ViewPath);
    }

    [MoqAutoData]
    public void Get_ViewModel_HasBackLink(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] PreviousEngagementController sut)
    {
        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.EngagedWithAPreviousAmbassadorInTheNetwork, Value = "True" });
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Model.As<PreviousEngagementViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Get_ViewModel_SelectedYes_RestoreFromSession(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] PreviousEngagementController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.EngagedWithAPreviousAmbassadorInTheNetwork, Value = "True" });

        var result = sut.Get();

        result.As<ViewResult>().Model.As<PreviousEngagementViewModel>().EngagedWithAPreviousAmbassadorInTheNetwork.Should().BeTrue();
    }

    [MoqAutoData]
    public void Get_ViewModel_SelectedNo_RestoreFromSession(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] PreviousEngagementController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.EngagedWithAPreviousAmbassadorInTheNetwork, Value = "False" });

        var result = sut.Get();

        result.As<ViewResult>().Model.As<PreviousEngagementViewModel>().EngagedWithAPreviousAmbassadorInTheNetwork.Should().BeFalse();
    }
}