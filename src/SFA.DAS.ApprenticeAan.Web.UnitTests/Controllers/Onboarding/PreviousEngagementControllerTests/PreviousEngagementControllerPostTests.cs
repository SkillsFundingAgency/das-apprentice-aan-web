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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.PreviousEngagementControllerTests;

public class PreviousEngagementControllerPostTests
{
    [Test, MoqAutoData]
    public void Post_ModelStateIsInvalid_ReloadsViewWithValidationErrors(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] PreviousEngagementController sut,
        [Frozen] PreviousEngagementSubmitModel submitModel)
    {
        //Arrange
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(GetSessionModel(false));

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest);

        sut.ModelState.AddModelError("key", "message");

        //Action
        var result = sut.Post(submitModel);

        //Assert
        sut.ModelState.IsValid.Should().BeFalse();
        result.As<ViewResult>().Should().NotBeNull();
        result.As<ViewResult>().ViewName.Should().Be(PreviousEngagementController.ViewPath);
        result.As<ViewResult>().Model.As<PreviousEngagementViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public void Post_ModelStateIsValidAndHasSeenPreview_UpdatesSessionModel(
        bool hasPreviousEngagementValue,
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<PreviousEngagementSubmitModel>> validatorMock,
        [Greedy] PreviousEngagementController sut)
    {
        //Arrange
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers);

        PreviousEngagementSubmitModel submitModel = new() { HasPreviousEngagement = hasPreviousEngagementValue };
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());

        OnboardingSessionModel sessionModel = GetSessionModel(false);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        //Act
        sut.Post(submitModel);

        //Assert
        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(s => s.HasSeenPreview)));
        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileConstants.ProfileIds.EngagedWithAPreviousAmbassadorInTheNetworkApprentice)?.Value.Should().Be(hasPreviousEngagementValue.ToString());
    }

    [Test, MoqAutoData]
    public void Post_ModelStateIsValidAndHasSeenPreview_RedirectsToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<PreviousEngagementSubmitModel>> validatorMock,
        PreviousEngagementSubmitModel submitModel,
        [Greedy] PreviousEngagementController sut)
    {
        //Arrange
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest);
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(GetSessionModel(true));

        //Act
        var result = sut.Post(submitModel);

        //Arrange
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }

    private OnboardingSessionModel GetSessionModel(bool hasSeenPreview = true)
    {
        OnboardingSessionModel model = new() { HasSeenPreview = hasSeenPreview };
        model.ProfileData.Add(new ProfileModel { Id = ProfileConstants.ProfileIds.EngagedWithAPreviousAmbassadorInTheNetworkApprentice, Value = null });
        return model;
    }
}