using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
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
public class CurrentJobTitleControllerPostTests
{
    [MoqAutoData]
    public void Post_SetsEnteredJobTitleInOnBoardingSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<CurrentJobTitleSubmitModel>> validatorMock,
        [Greedy] CurrentJobTitleController sut,
        CurrentJobTitleSubmitModel submitmodel,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        ValidationResult validationResult = new();
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        sut.Post(submitmodel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.GetProfileValue(ProfileDataId.JobTitle) == submitmodel.JobTitle)));
    }

    [MoqAutoData]
    public void Post_ModelStateIsValid_RedirectsToRegionsView(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<CurrentJobTitleSubmitModel>> validatorMock,
        [Greedy] CurrentJobTitleController sut,
        CurrentJobTitleSubmitModel submitmodel,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock();
        sessionModel.HasSeenPreview = false;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        ValidationResult validationResult = new();
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        var result = sut.Post(submitmodel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.GetProfileValue(ProfileDataId.JobTitle) == submitmodel.JobTitle)));

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.Regions);
    }

    [MoqAutoData]
    public void Post_ChangeJobTitle_RedirectsToCheckYourAnswersView(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<CurrentJobTitleSubmitModel>> validatorMock,
        [Greedy] CurrentJobTitleController sut,
        CurrentJobTitleSubmitModel submitmodel,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock();
        sessionModel.HasSeenPreview = true;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        ValidationResult validationResult = new();
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        var result = sut.Post(submitmodel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.GetProfileValue(ProfileDataId.JobTitle) == submitmodel.JobTitle)));

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
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        CurrentJobTitleSubmitModel submitmodel = new();
        submitmodel.JobTitle = null;

        sut.Post(submitmodel);

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
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        CurrentJobTitleSubmitModel submitmodel = new();
        submitmodel.JobTitle = null;

        var result = sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeFalse();
        result.As<ViewResult>().Model.As<CurrentJobTitleViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Post_BackLink_RedirectsRouteToEmployerDetails(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CurrentJobTitleController sut,
        OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.EmployerDetails);
        sessionModel.HasSeenPreview = false;
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.JobTitle, Value = "Some Title" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        CurrentJobTitleSubmitModel submitmodel = new();
        submitmodel.JobTitle = null;

        var result = sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeFalse();
        result.As<ViewResult>().Model.As<CurrentJobTitleViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }
}