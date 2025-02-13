using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.ReceiveNotificationsControllerTests;

[TestFixture]
public class ReceiveNotificationsControllerPostTests
{
    [Test, MoqAutoData]
    public void Post_UpdatesSessionModel(
        [Frozen] Mock<IValidator<Web.Models.Shared.ReceiveNotificationsSubmitModel>> mockValidator,
        [Frozen] Mock<ISessionService> mockSessionService,
        Web.Models.Shared.ReceiveNotificationsSubmitModel submitModel,
        ValidationResult validationResult,
        OnboardingSessionModel sessionModel,
        [Greedy] ReceiveNotificationsController controller)
    {
        sessionModel.ReceiveNotifications = null;
        submitModel.ReceiveNotifications = true;
        validationResult.Errors.Clear();
        mockValidator.Setup(v => v.Validate(submitModel)).Returns(validationResult);
        mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = controller.Post(submitModel, CancellationToken.None) as RedirectToRouteResult;

        mockSessionService.Verify(x => x.Set(It.Is<OnboardingSessionModel>(s => s.ReceiveNotifications == submitModel.ReceiveNotifications)), Times.Once);
    }

    [TestCase(false, null, RouteNames.Onboarding.PreviousEngagement)]
    [TestCase(false, false, RouteNames.Onboarding.PreviousEngagement)]
    [TestCase(false, true, RouteNames.Onboarding.CheckYourAnswers)]
    [TestCase(true, null, RouteNames.Onboarding.SelectNotificationEvents)]
    [TestCase(true, false, RouteNames.Onboarding.SelectNotificationEvents)]
    [TestCase(true, true, RouteNames.Onboarding.CheckYourAnswers)]
    public void Post_RedirectsToCorrectRoute(bool newValue, bool? previousValue, string expectedRouteName)
    {
        var validator = new Mock<IValidator<Web.Models.Shared.ReceiveNotificationsSubmitModel>>();
        var sessionService = new Mock<ISessionService>();

        var sessionModel = new OnboardingSessionModel
        {
            ReceiveNotifications = previousValue,
            HasSeenPreview = previousValue == true
        };

        validator.Setup(v => v.Validate(It.IsAny<Web.Models.Shared.ReceiveNotificationsSubmitModel>())).Returns(new ValidationResult());
        sessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var controller = new ReceiveNotificationsController(sessionService.Object, validator.Object);

        var submitModel = new Web.Models.Shared.ReceiveNotificationsSubmitModel
        {
            ReceiveNotifications = newValue
        };

        var result = controller.Post(submitModel, CancellationToken.None);

        result.Should().BeOfType<RedirectToRouteResult>();
        var redirectResult = (RedirectToRouteResult)result;

        redirectResult.RouteName.Should().Be(expectedRouteName);
    }
}


