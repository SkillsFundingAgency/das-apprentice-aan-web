using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CurrentJobTitleControllerTests;

[TestFixture]
public class CurrentJobTitleControllerPostTests
{
    [MoqAutoData]
    public void Post_SetsEnteredJobTitleInOnBoardingSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<CurrentJobTitleSubmitModel>> validatorMock,
        [Greedy] CurrentJobTitleController sut,
        CurrentJobTitleSubmitModel submitModel,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        ValidationResult validationResult = new();
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(validationResult);

        sut.Post(submitModel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.GetProfileValue(ProfileConstants.ProfileIds.JobTitle) == submitModel.JobTitle)));
    }

    [MoqAutoData]
    public void Post_ModelStateIsValid_RedirectsToRegionsView(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<CurrentJobTitleSubmitModel>> validatorMock,
        [Greedy] CurrentJobTitleController sut,
        CurrentJobTitleSubmitModel submitModel,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock();
        sessionModel.HasSeenPreview = false;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        ValidationResult validationResult = new();
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(validationResult);

        var result = sut.Post(submitModel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.GetProfileValue(ProfileConstants.ProfileIds.JobTitle) == submitModel.JobTitle)));

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.Regions);
    }

    [MoqAutoData]
    public void Post_ChangeJobTitle_RedirectsToCheckYourAnswersView(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<CurrentJobTitleSubmitModel>> validatorMock,
        [Greedy] CurrentJobTitleController sut,
        CurrentJobTitleSubmitModel submitModel,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock();
        sessionModel.HasSeenPreview = true;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        ValidationResult validationResult = new();
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(validationResult);

        var result = sut.Post(submitModel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.GetProfileValue(ProfileConstants.ProfileIds.JobTitle) == submitModel.JobTitle)));

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }

    [MoqAutoData]
    public void Post_Errors_WhenEnteredJobTitleIsNull(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CurrentJobTitleController sut,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock();
        sessionModel.HasSeenPreview = false;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        CurrentJobTitleSubmitModel submitModel = new();
        submitModel.JobTitle = null;

        sut.Post(submitModel);

        sut.ModelState.IsValid.Should().BeFalse();
    }

    [MoqAutoData]
    public void Post_BackLink_RedirectsRouteToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CurrentJobTitleController sut,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers);
        sessionModel.HasSeenPreview = true;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        CurrentJobTitleSubmitModel submitModel = new();
        submitModel.JobTitle = null;

        var result = sut.Post(submitModel);

        sut.ModelState.IsValid.Should().BeFalse();
        result.As<ViewResult>().Model.As<CurrentJobTitleViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Post_BackLink_RedirectsRouteToEmployerDetails(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CurrentJobTitleController sut,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.EmployerSearch);
        sessionModel.HasSeenPreview = false;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        CurrentJobTitleSubmitModel submitModel = new()
        {
            JobTitle = null
        };

        var result = sut.Post(submitModel);

        sut.ModelState.IsValid.Should().BeFalse();
        result.As<ViewResult>().Model.As<CurrentJobTitleViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }
}