﻿using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
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
    public void Post_SetsAreasOfInterestValuesInOnBoardingSessionModel(
    [Frozen] Mock<ISessionService> sessionServiceMock,
    [Frozen] Mock<IValidator<AreasOfInterestSubmitModel>> validatorMock,
    [Greedy] AreasOfInterestController sut,
    OnboardingSessionModel sessionModel)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.ReasonToJoin);

        sessionModel.ProfileData.Add(new ProfileModel { Id = 1, Value = null });
        sessionModel.ProfileData.Add(new ProfileModel { Id = 2, Value = true.ToString() });

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        AreasOfInterestSubmitModel submitmodel = new()
        {
            Events = new List<SelectProfileModel> { new SelectProfileModel { Id = 1, IsSelected = false } },
            Promotions = new List<SelectProfileModel> { new SelectProfileModel { Id = 2, IsSelected = true } }
        };

        ValidationResult validationResult = new();
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        sut.Post(submitmodel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.GetProfileValue(1) == null)));
        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.GetProfileValue(2) == true.ToString())));
    }

    [MoqAutoData]
    public void Post_WhenNoSelectionInAreasOfInterest_Errors(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] AreasOfInterestController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.ReasonToJoin);

        sessionModel.ProfileData.Add(new ProfileModel { Id = 1, Value = true.ToString() });
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        sut.ModelState.AddModelError("key", "message");

        AreasOfInterestSubmitModel submitmodel = new()
        {
            Events = new List<SelectProfileModel> { new SelectProfileModel { Id = 1, IsSelected = false } }
        };

        sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeFalse();
    }
}