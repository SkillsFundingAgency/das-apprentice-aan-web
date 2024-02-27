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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.AreasOfInterestControllerTests;

[TestFixture]
public class AreasOfInterestControllerPostTests
{
    [MoqAutoData]
    public void Post_SetsAreasOfInterestValuesInOnBoardingSessionModelRedirectsToPreviousEngagement(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<AreasOfInterestSubmitModel>> validatorMock,
        [Greedy] AreasOfInterestController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.ReasonToJoin);
        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = 1, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = 2, Value = true.ToString() });

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        AreasOfInterestSubmitModel submitmodel = new()
        {
            Events = [new SelectProfileModel { Id = 1, IsSelected = false }],
            Promotions = [new SelectProfileModel { Id = 2, IsSelected = true }]
        };

        ValidationResult validationResult = new();
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        var result = sut.Post(submitmodel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.GetProfileValue(1) == null)));
        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.GetProfileValue(2) == true.ToString())));

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.PreviousEngagement);
    }

    [MoqAutoData]
    public void Post_WhenNoSelectionInAreasOfInterest_Errors(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] AreasOfInterestController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.ReasonToJoin);

        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = 1, Value = true.ToString() });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        sut.ModelState.AddModelError("key", "message");

        AreasOfInterestSubmitModel submitmodel = new()
        {
            Events = [new SelectProfileModel { Id = 1, IsSelected = false }]
        };

        sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeFalse();
    }

    [MoqAutoData]
    public void Post_IsValidAndHasNotSeenPreview_RedirectsToPreviousEngagement(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<AreasOfInterestSubmitModel>> validatorMock,
        [Greedy] AreasOfInterestController sut,
        OnboardingSessionModel sessionModel)
    {
        ValidationResult validationResult = new();
        sessionModel.HasSeenPreview = false;

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.ReasonToJoin);//remove it

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        AreasOfInterestSubmitModel submitmodel = new();
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        submitmodel.Events = new List<SelectProfileModel>(sessionModel.ProfileData.Where(x => x.Category == Category.Events).Select(x => (SelectProfileModel)x));
        submitmodel.Promotions = new List<SelectProfileModel>(sessionModel.ProfileData.Where(x => x.Category == Category.Promotions).Select(x => (SelectProfileModel)x));

        var result = sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.PreviousEngagement);
    }

    [MoqAutoData]
    public void Post_IsValidAndHasSeenPreview_RedirectsToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<AreasOfInterestSubmitModel>> validatorMock,
        [Greedy] AreasOfInterestController sut,
        OnboardingSessionModel sessionModel)
    {
        ValidationResult validationResult = new();
        sessionModel.HasSeenPreview = true;

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.ReasonToJoin);//remove it

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        AreasOfInterestSubmitModel submitmodel = new();
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        submitmodel.Events = new List<SelectProfileModel>(sessionModel.ProfileData.Where(x => x.Category == Category.Events).Select(x => (SelectProfileModel)x));
        submitmodel.Promotions = new List<SelectProfileModel>(sessionModel.ProfileData.Where(x => x.Category == Category.Promotions).Select(x => (SelectProfileModel)x));

        var result = sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }
}