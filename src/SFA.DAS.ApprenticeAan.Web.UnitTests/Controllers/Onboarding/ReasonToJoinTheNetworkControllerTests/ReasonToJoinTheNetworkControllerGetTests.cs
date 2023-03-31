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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.ReasonToJoinTheNetworkControllerTests;

[TestFixture]
public class ReasonToJoinTheNetworkControllerGetTests
{
    [MoqAutoData]
    public void Get_ViewResult_HasCorrectViewPath(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] ReasonToJoinTheNetworkController sut)
    {
        sut.AddUrlHelperMock();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.EngagedWithAPreviousAmbassadorInTheNetwork, Value = "True" });

        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(ReasonToJoinTheNetworkController.ViewPath);
    }

    [MoqAutoData]
    public void Get_ViewModel_HasBackLink(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] ReasonToJoinTheNetworkController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.EngagedWithAPreviousAmbassadorInTheNetwork, Value = "True" });

        var result = sut.Get();

        result.As<ViewResult>().Model.As<ReasonToJoinTheNetworkViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Get_ViewModelHasEmployersApproval_RestoreFromSession(
                        [Frozen] Mock<ISessionService> sessionServiceMock,
                        OnboardingSessionModel sessionModel,
                        [Greedy] ReasonToJoinTheNetworkController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.EngagedWithAPreviousAmbassadorInTheNetwork, Value = "True" });

        var result = sut.Get();

        result.As<ViewResult>().Model.As<ReasonToJoinTheNetworkViewModel>().EngagedWithAPreviousAmbassadorInTheNetwork.Should().BeTrue();
    }
}