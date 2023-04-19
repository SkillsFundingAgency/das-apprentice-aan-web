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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.RegionsControllerTests;

[TestFixture]
public class RegionsControllerGetTests
{
    [MoqAutoData]
    public async Task Get_ReturnsViewResult([Greedy] RegionsController sut)
    {
        sut.AddUrlHelperMock();
        var result = await sut.Get();

        result.As<ViewResult>().Should().NotBeNull();
    }

    [MoqAutoData]
    public async Task Get_ViewResult_HasCorrectViewPath([Greedy] RegionsController sut)
    {
        sut.AddUrlHelperMock();
        var result = await sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(RegionsController.ViewPath);
    }

    [MoqAutoData]
    public async Task Get_ViewModelHasSeenPreviewIsTrue_BankLinkSetsToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] RegionsController sut,
        OnboardingSessionModel sessionModel,
        string checkYourAnswersUrl)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers, checkYourAnswersUrl);
        sessionModel.HasSeenPreview = true;
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = await sut.Get();

        result.As<ViewResult>().Model.As<RegionsViewModel>().BackLink.Should().Be(checkYourAnswersUrl);
    }

    [MoqAutoData]
    public async Task Get_ViewModelHasSeenPreviewIsFalse_BankLinkSetsToCurrentJobTitle(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] RegionsController sut,
        OnboardingSessionModel sessionModel,
        string currentJobTitleUrl)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle, currentJobTitleUrl);
        sessionModel.HasSeenPreview = false;
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = await sut.Get();

        result.As<ViewResult>().Model.As<RegionsViewModel>().BackLink.Should().Be(currentJobTitleUrl);
    }

    [MoqAutoData]
    public async Task Get_ViewModel_HasSessionData(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] RegionsController sut,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock();

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = await sut.Get();

        result.As<ViewResult>().Model.As<RegionsViewModel>().Regions.Should().NotBeNull();
        result.As<ViewResult>().Model.As<RegionsViewModel>().SelectedRegionId.Should().Be(sessionModel.RegionId);
    }
}