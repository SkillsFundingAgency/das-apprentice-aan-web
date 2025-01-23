using AutoFixture.NUnit3;

using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using FluentAssertions;
using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Models;
using FluentValidation.Results;
using SFA.DAS.ApprenticeAan.Web.Orchestrators;
using SFA.DAS.ApprenticeAan.Web.Controllers.EventNotificationSettings;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.EventNotificationSettings;

[TestFixture]
public class ReceiveNotificationsControllerGetTests
{
    [Test, MoqAutoData]
    public void Get_WhenCalled_ReturnsViewResultWithViewModel(
        [Frozen] Mock<ISessionService> mockSessionService,
        string settingsUri,
        NotificationSettingsSessionModel sessionModel,
        CancellationToken cancellationToken,
        [Greedy] ReceiveNotificationsController controller)
    {
        //Arrange
        mockSessionService.Setup(s => s.Get<NotificationSettingsSessionModel>()).Returns(sessionModel);

        controller.AddUrlHelperMock().AddUrlForRoute(RouteNames.EventNotificationSettings.Settings, settingsUri);

        // Act
        var result = controller.Get(cancellationToken) as ViewResult;

        // Assert
        result.Should().NotBeNull();
        var viewModel = result.Model as ReceiveNotificationsViewModel;
        viewModel.Should().NotBeNull();
        viewModel.ReceiveNotifications.Should().Be(sessionModel.ReceiveNotifications);
    }

    [Test, MoqAutoData]
    public async Task Post_UpdatesSessionModelAsync(
        [Frozen] Mock<IValidator<ReceiveNotificationsSubmitModel>> mockValidator,
        [Frozen] Mock<ISessionService> mockSessionService,
        ReceiveNotificationsSubmitModel submitModel,
        ValidationResult validationResult,
        NotificationSettingsSessionModel sessionModel,
        [Greedy] ReceiveNotificationsController controller)
    {
        // Arrange
        sessionModel.ReceiveNotifications = null;
        submitModel.ReceiveNotifications = true;
        validationResult.Errors.Clear();
        mockValidator.Setup(v => v.Validate(submitModel)).Returns(validationResult);
        mockSessionService.Setup(s => s.Get<NotificationSettingsSessionModel>()).Returns(sessionModel);

        // Act
        var result = await controller.Post(submitModel, CancellationToken.None) as RedirectToRouteResult;

        // Assert
        mockSessionService.Verify(x => x.Set(It.Is<NotificationSettingsSessionModel>(s => s.ReceiveNotifications == submitModel.ReceiveNotifications)), Times.Once);
    }

    [TestCase(false, null, RouteNames.EventNotificationSettings.Settings)]
    [TestCase(false, false, RouteNames.EventNotificationSettings.Settings)]
    [TestCase(false, true, RouteNames.EventNotificationSettings.Settings)]
    [TestCase(true, null, RouteNames.EventNotificationSettings.EventTypes)]
    [TestCase(true, false, RouteNames.EventNotificationSettings.EventTypes)]
    [TestCase(true, true, RouteNames.EventNotificationSettings.Settings)]
    public void Post_RedirectsToCorrectRoute(bool newValue, bool? previousValue, string expectedRouteName)
    {
        var validator = new Mock<IValidator<ReceiveNotificationsSubmitModel>>();
        var sessionService = new Mock<ISessionService>();
        var orchestrator = new Mock<IEventNotificationSettingsOrchestrator>();

        var sessionModel = new NotificationSettingsSessionModel
        {
            ReceiveNotifications = previousValue
        };

        validator.Setup(v => v.Validate(It.IsAny<ReceiveNotificationsSubmitModel>())).Returns(new ValidationResult());
        sessionService.Setup(s => s.Get<NotificationSettingsSessionModel>()).Returns(sessionModel);

        var controller = new ReceiveNotificationsController(validator.Object, orchestrator.Object, sessionService.Object);

        var submitModel = new ReceiveNotificationsSubmitModel
        {
            ReceiveNotifications = newValue
        };

        var result = controller.Post(submitModel, CancellationToken.None).Result;

        result.Should().BeOfType<RedirectToRouteResult>();
        var redirectResult = (RedirectToRouteResult)result;

        redirectResult.RouteName.Should().Be(expectedRouteName);
    }
}