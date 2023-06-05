using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.PreviousEngagementControllerTests;

public class PreviousEngagementControllerPostTests
{
    [Test, MoqAutoData]
    public async Task Post_ModelStateIsInvalid_ReloadsViewWithValidationErrors(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] PreviousEngagementController sut,
        [Frozen] PreviousEngagementSubmitModel submitModel,
        CancellationToken cancellationToken)
    {
        //Arrange
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(GetSessionModel(false));

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.AreasOfInterest);

        sut.ModelState.AddModelError("key", "message");

        //Action
        var result = await sut.Post(submitModel, cancellationToken);

        //Assert
        sut.ModelState.IsValid.Should().BeFalse();
        result.As<ViewResult>().Should().NotBeNull();
        result.As<ViewResult>().ViewName.Should().Be(PreviousEngagementController.ViewPath);
        result.As<ViewResult>().Model.As<PreviousEngagementViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public async Task Post_ModelStateIsValidAndHasSeenPreview_UpdatesSessionModel(
        bool hasPreviousEngagementValue,
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<PreviousEngagementSubmitModel>> validatorMock,
        [Greedy] PreviousEngagementController sut,
        CancellationToken cancellationToken)
    {
        //Arrange
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers);

        PreviousEngagementSubmitModel submitmodel = new() { HasPreviousEngagement = hasPreviousEngagementValue };
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(new ValidationResult());

        OnboardingSessionModel sessionModel = GetSessionModel(true);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        //Act
        await sut.Post(submitmodel, cancellationToken);

        //Assert
        sessionServiceMock.Verify(s => s.Set(sessionModel));
        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.HasPreviousEngagement)?.Value.Should().Be(hasPreviousEngagementValue.ToString());
    }

    [Test, MoqAutoData]
    public async Task Post_ModelStateIsValidAndHasSeenPreview_RedirectsToCheckYourAnswers(
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
        var result = await sut.Post(submitModel, new());

        //Arrange
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }

    [Test, MoqAutoData]
    public async Task Post_ModelStateIsValidAndHasNotSeenPreview_LoadsApprenticesDetails(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<PreviousEngagementSubmitModel>> validatorMock,
        [Frozen] Mock<IApprenticeAccountService> apprenticeAccountServiceMock,
        [Frozen] Mock<IMyApprenticeshipService> myApprenticeshipServiceMock,
        [Greedy] PreviousEngagementController sut,
        PreviousEngagementSubmitModel submitModel,
        Guid apprenticeId,
        ApprenticeAccount apprenticeAccount,
        MyApprenticeship myApprenticeship,
        CancellationToken cancellationToken)
    {
        //Arrange
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());

        var sessionModel = GetSessionModel(false);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(apprenticeId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        apprenticeAccountServiceMock.Setup(s => s.GetApprenticeAccountDetails(apprenticeId)).ReturnsAsync(apprenticeAccount);
        myApprenticeshipServiceMock.Setup(s => s.TryCreateMyApprenticeship(apprenticeId, apprenticeAccount.LastName, apprenticeAccount.Email, apprenticeAccount.DateOfBirth, cancellationToken)).ReturnsAsync(myApprenticeship);

        //Act
        await sut.Post(submitModel, cancellationToken);

        //Assert
        sessionModel.HasSeenPreview.Should().BeTrue();
        sessionModel.ApprenticeDetails.ApprenticeId.Should().Be(apprenticeId);
        sessionModel.ApprenticeDetails.Email.Should().Be(apprenticeAccount.Email);
        sessionModel.ApprenticeDetails.Name.Should().Be(user.FullName());
        sessionModel.MyApprenticeship.Should().Be(myApprenticeship);
    }

    private OnboardingSessionModel GetSessionModel(bool hasSeenPreview = true)
    {
        OnboardingSessionModel model = new() { HasSeenPreview = hasSeenPreview };
        model.ProfileData.Add(new ProfileModel { Id = ProfileDataId.HasPreviousEngagement, Value = null });
        return model;
    }
}