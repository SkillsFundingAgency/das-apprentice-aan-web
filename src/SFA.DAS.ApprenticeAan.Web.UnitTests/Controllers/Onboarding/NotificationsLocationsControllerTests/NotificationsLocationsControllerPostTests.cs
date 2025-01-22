using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using SFA.DAS.ApprenticeAan.Web.Orchestrators.Shared;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses.Shared;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.NotificationsLocationsControllerTests
{
    [TestFixture]
    public class NotificationsLocationsControllerPostTests
    {
        [Test, MoqAutoData]
        public async Task Post_SubmitButtonContinue_WithValidLocationsAlreadyAdded_ShouldRedirectToPreviousEngagement(
            [Frozen] Mock<ISessionService> mockSessionService,
            [Frozen] Mock<IValidator<INotificationsLocationsPartialSubmitModel>> mockValidator,
            [Frozen] Mock<IOuterApiClient> mockApiClient,
            NotificationsLocationsSubmitModel submitModel,
            OnboardingSessionModel sessionModel)
        {
            submitModel.SubmitButton = NotificationsLocationsSubmitButtonOption.Continue;
            submitModel.Location = "";
            sessionModel.NotificationLocations = new List<NotificationLocation> { new NotificationLocation { LocationName = "Test Location", Radius = 5 } };
            mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
            mockValidator.Setup(v => v.ValidateAsync(submitModel, default)).ReturnsAsync(new ValidationResult());

            var orchestrator = new NotificationsLocationsOrchestrator(mockSessionService.Object, mockValidator.Object, mockApiClient.Object);
            var controller = new NotificationsLocationsController(mockSessionService.Object, orchestrator, mockApiClient.Object);

            var result = await controller.Post(submitModel);

            var redirectResult = result as RedirectToRouteResult;
            redirectResult.Should().NotBeNull();
            redirectResult!.RouteName.Should().Be(RouteNames.Onboarding.PreviousEngagement);
        }

        [Test, MoqAutoData]
        public async Task Post_InvalidModel_ShouldRedirectToNotificationsLocations(
            [Frozen] Mock<ISessionService> mockSessionService,
            [Frozen] Mock<IValidator<INotificationsLocationsPartialSubmitModel>> mockValidator,
            NotificationsLocationsSubmitModel submitModel,
            OnboardingSessionModel sessionModel,
            [Greedy] NotificationsLocationsController controller)
        {
            submitModel.SubmitButton = NotificationsLocationsSubmitButtonOption.Add;
            submitModel.Location = "";
            mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
            mockValidator.Setup(v => v.ValidateAsync(submitModel, default)).ReturnsAsync(new ValidationResult
            {
                Errors = { new ValidationFailure(nameof(NotificationsLocationsSubmitModel.Location), "Location is required") }
            });

            var result = await controller.Post(submitModel);

            var redirectResult = result as RedirectToRouteResult;
            redirectResult.Should().NotBeNull();
            redirectResult!.RouteName.Should().Be(RouteNames.Onboarding.NotificationsLocations);
        }

        [Test, MoqAutoData]
        public async Task Post_LocationNotFound_ShouldReturnErrorAndRedirect(
            [Frozen] Mock<ISessionService> mockSessionService,
            [Frozen] Mock<IValidator<INotificationsLocationsPartialSubmitModel>> mockValidator,
            [Frozen] Mock<IOuterApiClient> mockApiClient,
            NotificationsLocationsSubmitModel submitModel,
            OnboardingSessionModel sessionModel)
        {
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Location", "test message") });
            submitModel.SubmitButton = NotificationsLocationsSubmitButtonOption.Add;
            submitModel.Location = "Unknown Location";
            mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
            mockValidator.Setup(v => v.ValidateAsync(submitModel, default)).ReturnsAsync(validationResult);
            mockApiClient.Setup(a => a.GetOnboardingNotificationsLocations(It.IsAny<string>()))
                .ReturnsAsync(new GetNotificationsLocationSearchApiResponse { Locations = new List<GetNotificationsLocationSearchApiResponse.Location>() });

            var orchestrator = new NotificationsLocationsOrchestrator(mockSessionService.Object, mockValidator.Object, mockApiClient.Object);
            var controller = new NotificationsLocationsController(mockSessionService.Object, orchestrator, mockApiClient.Object);

            var result = await controller.Post(submitModel);

            var redirectResult = result as RedirectToRouteResult;
            redirectResult.Should().NotBeNull();
            redirectResult!.RouteName.Should().Be(RouteNames.Onboarding.NotificationsLocations);
            controller.ModelState["Location"].Errors.Any(e => e.ErrorMessage == "test message");
        }

        [Test, MoqAutoData]
        public async Task Post_ValidLocation_Add_Clicked_ShouldAddToSessionAndReload(
            [Frozen] Mock<ISessionService> mockSessionService,
            [Frozen] Mock<IValidator<INotificationsLocationsPartialSubmitModel>> mockValidator,
            [Frozen] Mock<IOuterApiClient> mockApiClient,
            NotificationsLocationsSubmitModel submitModel,
            OnboardingSessionModel sessionModel)
        {
            submitModel.SubmitButton = NotificationsLocationsSubmitButtonOption.Add;
            submitModel.Location = "Valid Location";
            submitModel.Radius = 5;
            var apiResponse = new GetNotificationsLocationSearchApiResponse
            {
                Locations = new List<GetNotificationsLocationSearchApiResponse.Location> { new() { Name = "Valid Location" } }
            };
            mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
            mockValidator.Setup(v => v.ValidateAsync(submitModel, default)).ReturnsAsync(new ValidationResult());
            mockApiClient.Setup(a => a.GetOnboardingNotificationsLocations(It.IsAny<string>()))
                .ReturnsAsync(apiResponse);

            var orchestrator = new NotificationsLocationsOrchestrator(mockSessionService.Object, mockValidator.Object, mockApiClient.Object);
            var controller = new NotificationsLocationsController(mockSessionService.Object, orchestrator, mockApiClient.Object);

            var result = await controller.Post(submitModel);

            var redirectResult = result as RedirectToRouteResult;
            redirectResult.Should().NotBeNull();
            redirectResult!.RouteName.Should().Be(RouteNames.Onboarding.NotificationsLocations);

            mockSessionService.Verify(
                x => x.Set(It.Is<OnboardingSessionModel>(m =>
                    m.NotificationLocations.Any(n => n.LocationName == "Valid Location" && n.Radius == 5))),
                Times.Once);
        }


        [Test, MoqAutoData]
        public async Task Post_ValidLocation_Continue_Clicked_ShouldAddToSession_AndRedirect_Onward(
            [Frozen] Mock<ISessionService> mockSessionService,
            [Frozen] Mock<IValidator<INotificationsLocationsPartialSubmitModel>> mockValidator,
            [Frozen] Mock<IOuterApiClient> mockApiClient,
            NotificationsLocationsSubmitModel submitModel,
            OnboardingSessionModel sessionModel)
        {
            sessionModel.HasSeenPreview = false;
            submitModel.SubmitButton = NotificationsLocationsSubmitButtonOption.Continue;
            submitModel.Location = "Valid location";
            submitModel.Radius = 5;
            var apiResponse = new GetNotificationsLocationSearchApiResponse
            {
                Locations = new List<GetNotificationsLocationSearchApiResponse.Location> { new() { Name = "Valid Location" } }
            };
            mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
            mockValidator.Setup(v => v.ValidateAsync(submitModel, default)).ReturnsAsync(new ValidationResult());
            mockApiClient.Setup(a => a.GetOnboardingNotificationsLocations(It.IsAny<string>()))
                .ReturnsAsync(apiResponse);

            var orchestrator = new NotificationsLocationsOrchestrator(mockSessionService.Object, mockValidator.Object, mockApiClient.Object);
            var controller = new NotificationsLocationsController(mockSessionService.Object, orchestrator, mockApiClient.Object);

            var result = await controller.Post(submitModel);

            var redirectResult = result as RedirectToRouteResult;
            redirectResult.Should().NotBeNull();
            redirectResult!.RouteName.Should().Be(RouteNames.Onboarding.PreviousEngagement);

            mockSessionService.Verify(
                x => x.Set(It.Is<OnboardingSessionModel>(m =>
                    m.NotificationLocations.Any(n => n.LocationName == "Valid Location" && n.Radius == 5))),
                Times.Once);
        }


        [Test, MoqAutoData]
        public async Task Post_ValidLocation_Continue_Clicked_ShouldAddToSession_AndRedirect_To_CheckAnswers(
            [Frozen] Mock<ISessionService> mockSessionService,
            [Frozen] Mock<IValidator<INotificationsLocationsPartialSubmitModel>> mockValidator,
            [Frozen] Mock<IOuterApiClient> mockApiClient,
            NotificationsLocationsSubmitModel submitModel,
            OnboardingSessionModel sessionModel)
        {
            sessionModel.HasSeenPreview = true;
            submitModel.SubmitButton = NotificationsLocationsSubmitButtonOption.Continue;
            submitModel.Location = "Valid location";
            submitModel.Radius = 5;
            var apiResponse = new GetNotificationsLocationSearchApiResponse
            {
                Locations = new List<GetNotificationsLocationSearchApiResponse.Location> { new() { Name = "Valid Location" } }
            };
            mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
            mockValidator.Setup(v => v.ValidateAsync(submitModel, default)).ReturnsAsync(new ValidationResult());
            mockApiClient.Setup(a => a.GetOnboardingNotificationsLocations(It.IsAny<string>()))
                .ReturnsAsync(apiResponse);

            var orchestrator = new NotificationsLocationsOrchestrator(mockSessionService.Object, mockValidator.Object, mockApiClient.Object);
            var controller = new NotificationsLocationsController(mockSessionService.Object, orchestrator, mockApiClient.Object);


            var result = await controller.Post(submitModel);

            var redirectResult = result as RedirectToRouteResult;
            redirectResult.Should().NotBeNull();
            redirectResult!.RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);

            mockSessionService.Verify(
                x => x.Set(It.Is<OnboardingSessionModel>(m =>
                    m.NotificationLocations.Any(n => n.LocationName == "Valid Location" && n.Radius == 5))),
                Times.Once);
        }


        [Test, MoqAutoData]
        public async Task Post_SubmitButtonDelete_ShouldRemoveLocationAndRedirect(
            [Frozen] Mock<ISessionService> mockSessionService,
            [Frozen] Mock<IValidator<INotificationsLocationsPartialSubmitModel>> mockValidator,
            [Frozen] Mock<IOuterApiClient> mockApiClient,
            NotificationsLocationsSubmitModel submitModel,
            OnboardingSessionModel sessionModel)
        {
            submitModel.SubmitButton = NotificationsLocationsSubmitButtonOption.Delete + "-0";
            sessionModel.NotificationLocations = new List<NotificationLocation>
            {
                new NotificationLocation { LocationName = "Location to be deleted", Radius = 5 },
                new NotificationLocation { LocationName = "Another Location", Radius = 10 }
            };
            mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

            var orchestrator = new NotificationsLocationsOrchestrator(mockSessionService.Object, mockValidator.Object, mockApiClient.Object);
            var controller = new NotificationsLocationsController(mockSessionService.Object, orchestrator, mockApiClient.Object);

            var result = await controller.Post(submitModel);

            var redirectResult = result as RedirectToRouteResult;
            redirectResult.Should().NotBeNull();
            redirectResult!.RouteName.Should().Be(RouteNames.Onboarding.NotificationsLocations);

            sessionModel.NotificationLocations.Should().HaveCount(1);
            sessionModel.NotificationLocations.Should().NotContain(n => n.LocationName == "Location to be deleted");
            mockSessionService.Verify(s => s.Set(sessionModel), Times.Once);
        }
    }
}
