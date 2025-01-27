using AutoFixture.NUnit3;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.EventNotificationSettings;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Orchestrators.Shared;
using SFA.DAS.Testing.AutoFixture;
using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SFA.DAS.ApprenticeAan.Web.Models.EventNotificationSettings;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.EventNotificationSettings
{
    [TestFixture]
    public class EventNotificationSettingsLocationDisambiguationControllerPostTests
    {
        [Test, MoqAutoData]
        public async Task Post_ValidModel_UpdatesSessionAndRedirects(
           [Frozen] Mock<IValidator<NotificationLocationDisambiguationSubmitModel>> mockValidator,
           [Frozen] Mock<ISessionService> mockSessionService,
           [Frozen] Mock<INotificationLocationDisambiguationOrchestrator> mockOrchestrator,
           NotificationLocationDisambiguationSubmitModel submitModel,
           ValidationResult validationResult,
           NotificationSettingsSessionModel sessionModel,
           CancellationToken cancellationToken,
           [Greedy] EventNotificationSettingsLocationDisambiguationController controller)
        {
            validationResult.Errors.Clear();
            mockValidator.Setup(v => v.Validate(submitModel)).Returns(validationResult);
            mockSessionService.Setup(s => s.Get<NotificationSettingsSessionModel>()).Returns(sessionModel);

            mockOrchestrator
                .Setup(o => o.ApplySubmitModel<NotificationSettingsSessionModel>(submitModel, It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(NotificationLocationDisambiguationOrchestrator.RedirectTarget.NextPage);

            var result = await controller.Post(submitModel, cancellationToken) as RedirectToRouteResult;

            result.Should().NotBeNull();
            result!.RouteName.Should().Be(RouteNames.EventNotificationSettings.NotificationLocations);

            mockOrchestrator.Verify(o => o.ApplySubmitModel<NotificationSettingsSessionModel>(submitModel, It.IsAny<ModelStateDictionary>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Post_InvalidModel_RedirectsToSelf(
            [Frozen] Mock<IValidator<NotificationLocationDisambiguationSubmitModel>> mockValidator,
            [Frozen] Mock<ISessionService> mockSessionService,
            [Frozen] Mock<INotificationLocationDisambiguationOrchestrator> mockOrchestrator,
            NotificationLocationDisambiguationSubmitModel submitModel,
            ValidationResult validationResult,
            NotificationSettingsSessionModel sessionModel,
            CancellationToken cancellationToken,
            [Greedy] EventNotificationSettingsLocationDisambiguationController controller)
        {
            validationResult.Errors.Add(new ValidationFailure("Location", "Location is required"));
            mockValidator.Setup(v => v.Validate(submitModel)).Returns(validationResult);
            mockSessionService.Setup(s => s.Get<NotificationSettingsSessionModel>()).Returns(sessionModel);

            mockOrchestrator
                .Setup(o => o.ApplySubmitModel<NotificationSettingsSessionModel>(submitModel, It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(NotificationLocationDisambiguationOrchestrator.RedirectTarget.Self);

            var result = await controller.Post(submitModel, cancellationToken) as RedirectToRouteResult;

            result.Should().NotBeNull();
            result!.RouteName.Should().Be(RouteNames.EventNotificationSettings.SettingsNotificationLocationDisambiguation);
            result.RouteValues["radius"].Should().Be(submitModel.Radius);
            result.RouteValues["location"].Should().Be(submitModel.Location);

            mockOrchestrator.Verify(o => o.ApplySubmitModel<NotificationSettingsSessionModel>(submitModel, It.IsAny<ModelStateDictionary>()), Times.Once);
        }
    }
}
