using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.JoinTheNetworkControllerTests;

[TestFixture]
public class JoinTheNetworkControllerGetTests
{
    [MoqAutoData]
    public void Get_ReturnsViewResult([Greedy] JoinTheNetworkController sut)
    {
        sut.AddUrlHelperMock();
        var result = sut.Get();

        result.As<ViewResult>().Should().NotBeNull();
    }

    [MoqAutoData]
    public void Get_ViewResult_HasCorrectViewPath([Greedy] JoinTheNetworkController sut)
    {
        sut.AddUrlHelperMock();
        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(JoinTheNetworkController.ViewPath);
    }

    [MoqAutoData]
    public void Get_ViewModel_HasBackLink([Greedy] JoinTheNetworkController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.TermsAndConditions);
        var result = sut.Get();

        result.As<ViewResult>().Model.As<JoinTheNetworkViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Get_ViewModelHasEmployersApproval_RestoreFromSession(
                        [Frozen] Mock<ISessionService> sessionServiceMock,
                        OnboardingSessionModel sessionModel,
                        [Greedy] JoinTheNetworkController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.TermsAndConditions);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Model.As<JoinTheNetworkViewModel>().ReasonForJoiningTheNetwork.Should().Be(sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork);
    }
}