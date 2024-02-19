using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.ReasonToJoinControllerTests;

[TestFixture]
public class ReasonToJoinControllerPostTests
{
    [MoqAutoData]
    public void Post_ModelStateIsInvalid_ReloadsViewWithValidationErrors(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] ReasonToJoinController sut,
        [Frozen] ReasonToJoinSubmitModel submitModel,
        string reasonForJoining)
    {
        OnboardingSessionModel sessionModel = new();
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.Regions);

        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork, Value = reasonForJoining });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        sut.ModelState.AddModelError("key", "message");

        var result = sut.Post(submitModel);

        using (new AssertionScope())
        {
            sut.ModelState.IsValid.Should().BeFalse();
            result.As<ViewResult>().Should().NotBeNull();
            result.As<ViewResult>().ViewName.Should().Be(ReasonToJoinController.ViewPath);
            result.As<ViewResult>().Model.As<ReasonToJoinViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
            result.As<ViewResult>().Model.As<ReasonToJoinViewModel>().ReasonForJoiningTheNetwork.Should().Be(reasonForJoining);
        }
    }

    [MoqAutoData]
    public void Post_ModelStateIsValid_UpdatesSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<ReasonToJoinSubmitModel>> validatorMock,
        [Frozen] ReasonToJoinSubmitModel submitModel,
        [Greedy] ReasonToJoinController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.Regions);

        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork, Value = null });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(validationResult);

        sessionServiceMock.Object.Set(sessionModel);

        sut.Post(submitModel);

        sessionServiceMock.Verify(s => s.Set(sessionModel));

        sessionModel.GetProfileValue(ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork).Should().Be(submitModel.ReasonForJoiningTheNetwork);

        sut.ModelState.IsValid.Should().BeTrue();
    }

    [MoqAutoData]
    public void Post_ModelStateIsValidAndHasNotSeenPreview_RedirectsToAreasOfInterest(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<ReasonToJoinSubmitModel>> validatorMock,
        [Frozen] ReasonToJoinSubmitModel submitModel,
        [Greedy] ReasonToJoinController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();
        sessionModel.HasSeenPreview = false;

        sut.AddUrlHelperMock();

        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork, Value = null });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(validationResult);

        var result = sut.Post(submitModel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.AreasOfInterest);
    }

    [MoqAutoData]
    public void Post_IsValidAndHasSeenPreview_RedirectsToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<ReasonToJoinSubmitModel>> validatorMock,
        [Frozen] ReasonToJoinSubmitModel submitModel,
        [Greedy] ReasonToJoinController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();

        sessionModel.HasSeenPreview = true;
        sut.AddUrlHelperMock();

        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.ReasonToJoinAmbassadorNetwork, Value = null });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(validationResult);

        var result = sut.Post(submitModel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }
}