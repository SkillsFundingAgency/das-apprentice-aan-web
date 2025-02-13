using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Orchestrators.Shared;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.NotificationLocationDisambiguationControllerTests
{
    [TestFixture]
    public class NotificationLocationDisambiguationControllerPostTests
    {
        [Test, MoqAutoData]
        public async Task Post_ValidModel_UpdatesSessionAndRedirects(
            [Frozen] Mock<IValidator<NotificationLocationDisambiguationSubmitModel>> mockValidator,
            [Frozen] Mock<ISessionService> mockSessionService,
            [Frozen] Mock<INotificationLocationDisambiguationOrchestrator> mockOrchestrator,
            NotificationLocationDisambiguationSubmitModel submitModel,
            ValidationResult validationResult,
            OnboardingSessionModel sessionModel,
            CancellationToken cancellationToken,
            [Greedy] NotificationLocationDisambiguationController controller)
        {
            validationResult.Errors.Clear();
            mockValidator.Setup(v => v.Validate(submitModel)).Returns(validationResult);
            mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

            mockOrchestrator
                .Setup(o => o.ApplySubmitModel<OnboardingSessionModel>(submitModel, It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(NotificationLocationDisambiguationOrchestrator.RedirectTarget.NextPage);

            var result = await controller.Post(submitModel, cancellationToken) as RedirectToRouteResult;

            result.Should().NotBeNull();
            result!.RouteName.Should().Be(RouteNames.Onboarding.NotificationsLocations);

            mockOrchestrator.Verify(o => o.ApplySubmitModel<OnboardingSessionModel>(submitModel, It.IsAny<ModelStateDictionary>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Post_InvalidModel_RedirectsToSelf(
            [Frozen] Mock<IValidator<NotificationLocationDisambiguationSubmitModel>> mockValidator,
            [Frozen] Mock<ISessionService> mockSessionService,
            [Frozen] Mock<INotificationLocationDisambiguationOrchestrator> mockOrchestrator,
            NotificationLocationDisambiguationSubmitModel submitModel,
            ValidationResult validationResult,
            OnboardingSessionModel sessionModel,
            CancellationToken cancellationToken,
            [Greedy] NotificationLocationDisambiguationController controller)
        {
            validationResult.Errors.Add(new ValidationFailure("Location", "Location is required"));
            mockValidator.Setup(v => v.Validate(submitModel)).Returns(validationResult);
            mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

            mockOrchestrator
                .Setup(o => o.ApplySubmitModel<OnboardingSessionModel>(submitModel, It.IsAny<ModelStateDictionary>()))
                .ReturnsAsync(NotificationLocationDisambiguationOrchestrator.RedirectTarget.Self);

            var result = await controller.Post(submitModel, cancellationToken) as RedirectToRouteResult;

            result.Should().NotBeNull();
            result!.RouteName.Should().Be(RouteNames.Onboarding.NotificationLocationDisambiguation);
            result.RouteValues["radius"].Should().Be(submitModel.Radius);
            result.RouteValues["location"].Should().Be(submitModel.Location);

            mockOrchestrator.Verify(o => o.ApplySubmitModel<OnboardingSessionModel>(submitModel, It.IsAny<ModelStateDictionary>()), Times.Once);
        }
    }
}
