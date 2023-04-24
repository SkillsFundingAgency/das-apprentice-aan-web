﻿using AutoFixture.NUnit3;
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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.PreviousEngagementControllerTests;

[TestFixture]
public class PreviousEngagementControllerPostTests
{
    [MoqAutoData]
    public void Post_ModelStateIsInvalid_ReloadsViewWithValidationErrors(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] PreviousEngagementController sut,
        [Frozen] PreviousEngagementSubmitModel submitmodel)
    {
        OnboardingSessionModel sessionModel = new();
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.HasPreviousEngagement, Value = null });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest);

        sut.ModelState.AddModelError("key", "message");

        var result = sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeFalse();

        result.As<ViewResult>().Should().NotBeNull();
        result.As<ViewResult>().ViewName.Should().Be(PreviousEngagementController.ViewPath);
        result.As<ViewResult>().Model.As<PreviousEngagementViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [TestCase("true")]
    [TestCase("false")]
    public void Post_ModelStateIsValid_UpdatesSessionModel(string? hasPreviousEngagementValue)
    {
        Mock<ISessionService> sessionServiceMock = new();
        Mock<IValidator<PreviousEngagementSubmitModel>> validatorMock = new();
        PreviousEngagementSubmitModel submitmodel = new();
        PreviousEngagementController sut = new PreviousEngagementController(sessionServiceMock.Object, validatorMock.Object);
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest);

        submitmodel.HasPreviousEngagement = Convert.ToBoolean(hasPreviousEngagementValue);
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.HasPreviousEngagement, Value = null });

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        sessionServiceMock.Object.Set(sessionModel);

        sut.Post(submitmodel);

        sessionServiceMock.Verify(s => s.Set(sessionModel));
        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.HasPreviousEngagement)?.Value.Should().Be(submitmodel.HasPreviousEngagement.ToString());
        sut.ModelState.IsValid.Should().BeTrue();
    }

    [MoqAutoData]
    public void Post_ModelStateIsValid_RedirectsToCheckYourAnswersw(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<PreviousEngagementSubmitModel>> validatorMock,
        PreviousEngagementSubmitModel submitmodel,
        [Greedy] PreviousEngagementController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest);

        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.HasPreviousEngagement, Value = "True" });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        var result = sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }
}