using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.ReasonToJoinControllerTests;

[TestFixture]
public class ReasonToJoinControllerGetTests
{
    [Test, MoqAutoData]
    public void Get_ViewResult_HasCorrectViewPath(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] ReasonToJoinController sut,
        OnboardingSessionModel sessionModel,
        string reasonForJoining)
    {
        sessionModel.ProfileData.Clear();
        sut.AddUrlHelperMock();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork, Value = reasonForJoining });
        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(ReasonToJoinController.ViewPath);
    }

    [Test, MoqAutoData]
    public void Get_ViewModelHasEmployersApproval_RestoreFromSession(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] ReasonToJoinController sut,
        string reasonForJoining)
    {
        sessionModel.ProfileData.Clear();
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork, Value = reasonForJoining });

        var result = sut.Get();

        result.As<ViewResult>().Model.As<ReasonToJoinViewModel>().ReasonForJoiningTheNetwork.Should().Be(reasonForJoining);
    }

    [Test, MoqAutoData]
    public void Get_ViewModelHasSeenPreviewIsTrue_BackLinkSetsToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] ReasonToJoinController sut,
        OnboardingSessionModel sessionModel,
        string checkYourAnswersUrl,
        string reasonForJoining)
    {
        sessionModel.ProfileData.Clear();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork, Value = reasonForJoining });
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers, checkYourAnswersUrl);
        sessionModel.HasSeenPreview = true;
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Model.As<ReasonToJoinViewModel>().BackLink.Should().Be(checkYourAnswersUrl);
    }

    [Test, MoqAutoData]
    public void Get_ViewModelHasSeenPreviewIsFalse_BackLinkSetsToRegions(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] ReasonToJoinController sut,
        OnboardingSessionModel sessionModel,
        string areasOfInterestUrl,
        string reasonForJoining)
    {
        sessionModel.ProfileData.Clear();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork, Value = reasonForJoining });
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest, areasOfInterestUrl);
        sessionModel.HasSeenPreview = false;
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Model.As<ReasonToJoinViewModel>().BackLink.Should().Be(areasOfInterestUrl);
    }
}