using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.ReasonToJoinControllerTests;

[TestFixture]
public class ReasonToJoinControllerPostTests
{
    [MoqAutoData]
    public void Post_ModelStateIsInvalid_ReloadsViewWithValidationErrors(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] ReasonToJoinController sut,
        [Frozen] ReasonToJoinSubmitModel submitmodel)
    {
        OnboardingSessionModel sessionModel = new();
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.Regions);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        sut.ModelState.AddModelError("key", "message");

        var result = sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeFalse();

        result.As<ViewResult>().Should().NotBeNull();
        result.As<ViewResult>().ViewName.Should().Be(ReasonToJoinController.ViewPath);
        result.As<ViewResult>().Model.As<ReasonToJoinViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Post_ModelStateIsValid_UpdatesSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<ReasonToJoinSubmitModel>> validatorMock,
        [Frozen] ReasonToJoinSubmitModel submitmodel,
        [Greedy] ReasonToJoinController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.Regions);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        sessionServiceMock.Object.Set(sessionModel);

        sut.Post(submitmodel);

        sessionServiceMock.Verify(s => s.Set(sessionModel));

        sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork.Should().Be(submitmodel.ReasonForJoiningTheNetwork);

        sut.ModelState.IsValid.Should().BeTrue();
    }

    [MoqAutoData]
    public void Post_ModelStateIsValidAndHasNotSeenPreview_RedirectsToAreasOfInterest(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<ReasonToJoinSubmitModel>> validatorMock,
        [Frozen] ReasonToJoinSubmitModel submitmodel,
        [Greedy] ReasonToJoinController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();
        sessionModel.HasSeenPreview = false;

        sut.AddUrlHelperMock();

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        var result = sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.AreasOfInterest);
    }

    [MoqAutoData]
    public void Post_IsValidAndHasSeenPreview_RedirectsToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<ReasonToJoinSubmitModel>> validatorMock,
        [Frozen] ReasonToJoinSubmitModel submitmodel,
        [Greedy] ReasonToJoinController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();

        sessionModel.HasSeenPreview = true;
        sut.AddUrlHelperMock();

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        var result = sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }
}