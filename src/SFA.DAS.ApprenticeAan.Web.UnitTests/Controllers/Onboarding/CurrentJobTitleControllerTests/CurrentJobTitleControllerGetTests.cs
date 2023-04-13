﻿using AutoFixture.NUnit3;
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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CurrentJobTitleControllerTests;

[TestFixture]
public class CurrentJobTitleControllerGetTests
{
    [MoqAutoData]
    public void Get_ReturnsViewResult(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CurrentJobTitleController sut,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Should().NotBeNull();
    }

    [MoqAutoData]
    public void Get_ViewResult_HasCorrectViewPath(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CurrentJobTitleController sut,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(CurrentJobTitleController.ViewPath);
    }

    [MoqAutoData]
    public void Get_ViewModelHasSeenPreview_RedirectsRouteToCheckYourAnswers(
    [Frozen] Mock<ISessionService> sessionServiceMock,
    [Greedy] CurrentJobTitleController sut,
    OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Model.As<CurrentJobTitleViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Get_ViewModelHasSeenPreview_RedirectsRouteToEmployerDetails(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CurrentJobTitleController sut,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.EmployerDetails);
        sessionModel.HasSeenPreview = false;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Model.As<CurrentJobTitleViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Get_ViewModel_HasSessionData(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] CurrentJobTitleController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.EmployerDetails);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Model.As<CurrentJobTitleViewModel>().JobTitle.Should().Be(sessionModel.GetProfileValue(ProfileDataId.JobTitle));
    }
}