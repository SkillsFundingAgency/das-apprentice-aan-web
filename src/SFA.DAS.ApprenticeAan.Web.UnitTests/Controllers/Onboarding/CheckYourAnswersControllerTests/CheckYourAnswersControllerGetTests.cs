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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CheckYourAnswersControllerTests;

[TestFixture]
public class CheckYourAnswersControllerGetTests
{
    [MoqAutoData]
    public void Get_ReturnsViewResult(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CheckYourAnswersController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);
        var jobTitle = "Test Job Title";
        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = jobTitle });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.HasSeenPreview)));
        result.As<ViewResult>().Model.As<CheckYourAnswersViewModel>().JobTitle.Should().Be(jobTitle);
        result.As<ViewResult>().Model.As<CheckYourAnswersViewModel>().JobTitleChangeLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void JobTitleChangeLink_LinksToJobTitleRoute(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CheckYourAnswersController sut,
        OnboardingSessionModel sessionModel)
    {
        var jobTitle = "Some Title";
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = jobTitle });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.HasSeenPreview)));
        result.As<ViewResult>().Model.As<CheckYourAnswersViewModel>().JobTitle.Should().Be(jobTitle);
        result.As<ViewResult>().Model.As<CheckYourAnswersViewModel>().JobTitleChangeLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Index_ReturnsViewResult_ValidRegions(
    [Frozen] Mock<ISessionService> sessionServiceMock,
    [Greedy] CheckYourAnswersController sut,
    OnboardingSessionModel sessionModel)
    {
        var regionName = "London";
        var jobTitle = "Some Title";

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.Regions);
        sessionModel.RegionName = regionName;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = jobTitle });

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Index();

        result.As<ViewResult>().Model.As<CheckYourAnswersViewModel>().Region.Should().Be(regionName);
        result.As<ViewResult>().Model.As<CheckYourAnswersViewModel>().RegionChangeLink.Should().Be(TestConstants.DefaultUrl);
    }
}