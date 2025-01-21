using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using SFA.DAS.ApprenticeAan.Web.Orchestrators.Shared;
using SFA.DAS.ApprenticeAan.Web.Models.Shared;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Constant;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.NotificationsLocationsControllerTests
{
    [TestFixture]
    public class NotificationsLocationsControllerGetTests
    {
        [TestCase(true, false, false, "Add locations for in-person events")]
        [TestCase(false, true, false, "Add locations for hybrid events")]
        [TestCase(true, true, false, "Add locations for in-person and hybrid events")]
        [TestCase(false, false, true, "Add locations for in-person and hybrid events")]
        [TestCase(true, true, true, "Add locations for in-person and hybrid events")]
        public void Get_WhenCalled_And_No_Locations_Added_ReturnsViewModel_With_Correct_Title(bool inPerson, bool hybrid, bool all, string expectedPageTitle)
        {
            var fixture = new Fixture();
            var r = new Random();
            fixture.Register<bool>(() => r.NextDouble() < 0.5);

            var online = fixture.Create<bool>();

            var mockSessionService = new Mock<ISessionService>();
            var mockApiClient = new Mock<IOuterApiClient>();
            var mockValidator = new Mock<IValidator<INotificationsLocationsPartialSubmitModel>>();
            var orchestrator = new NotificationsLocationsOrchestrator(mockSessionService.Object, mockValidator.Object, mockApiClient.Object);
            var controller = new NotificationsLocationsController(mockSessionService.Object, orchestrator, mockApiClient.Object);

            controller.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers, "");
            controller.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.SelectNotificationEvents, "");

            var sessionModel = CreateSessionModel(inPerson, hybrid, online, all);
            mockSessionService.Setup(x => x.Get<OnboardingSessionModel>()).Returns(sessionModel);

            var result = controller.Get() as ViewResult;

            result.Should().NotBeNull();
            var viewModel = result.Model as NotificationsLocationsViewModel;
            viewModel.Should().NotBeNull();
            viewModel.Title.Should().Be(expectedPageTitle);
        }

        [TestCase(true, false, false, "Notifications for in-person events")]
        [TestCase(false, true, false, "Notifications for hybrid events")]
        [TestCase(true, true, false, "Notifications for in-person and hybrid events")]
        [TestCase(false, false, true, "Notifications for in-person and hybrid events")]
        [TestCase(true, true, true, "Notifications for in-person and hybrid events")]
        public void Get_WhenCalled_And_Locations_Added_ReturnsViewModel_With_Correct_Title(bool inPerson, bool hybrid, bool all, string expectedPageTitle)
        {
            var fixture = new Fixture();
            var r = new Random();
            fixture.Register<bool>(() => r.NextDouble() < 0.5);

            var online = fixture.Create<bool>();

            var mockSessionService = new Mock<ISessionService>();
            var mockApiClient = new Mock<IOuterApiClient>();
            var mockValidator = new Mock<IValidator<INotificationsLocationsPartialSubmitModel>>();
            var orchestrator = new NotificationsLocationsOrchestrator(mockSessionService.Object, mockValidator.Object, mockApiClient.Object);
            var controller = new NotificationsLocationsController(mockSessionService.Object, orchestrator, mockApiClient.Object);
            controller.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers, "");
            controller.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.SelectNotificationEvents, "");

            var sessionModel = CreateSessionModel(inPerson, hybrid, online, all);
            sessionModel.NotificationLocations.Add(new NotificationLocation { LocationName = "Test", Radius = 1 });
            mockSessionService.Setup(x => x.Get<OnboardingSessionModel>()).Returns(sessionModel);

            var result = controller.Get() as ViewResult;

            result.Should().NotBeNull();
            var viewModel = result.Model as NotificationsLocationsViewModel;
            viewModel.Should().NotBeNull();
            viewModel.Title.Should().Be(expectedPageTitle);
        }


        [TestCase(true, false, false, "Tell us where you want to hear about upcoming in-person events.")]
        [TestCase(false, true, false, "Tell us where you want to hear about upcoming hybrid events.")]
        [TestCase(true, true, false, "Tell us where you want to hear about upcoming in-person and hybrid events.")]
        [TestCase(false, false, true, "Tell us where you want to hear about upcoming in-person and hybrid events.")]
        [TestCase(true, true, true, "Tell us where you want to hear about upcoming in-person and hybrid events.")]
        public void Get_WhenCalled_ReturnsViewModel_With_Correct_IntroText(bool inPerson, bool hybrid, bool all, string expectedIntroText)
        {
            var fixture = new Fixture();
            var r = new Random();
            fixture.Register<bool>(() => r.NextDouble() < 0.5);

            var online = fixture.Create<bool>();

            var mockSessionService = new Mock<ISessionService>();
            var mockApiClient = new Mock<IOuterApiClient>();
            var mockValidator = new Mock<IValidator<INotificationsLocationsPartialSubmitModel>>();
            var orchestrator = new NotificationsLocationsOrchestrator(mockSessionService.Object, mockValidator.Object, mockApiClient.Object);
            var controller = new NotificationsLocationsController(mockSessionService.Object, orchestrator, mockApiClient.Object);
            controller.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers, "");
            controller.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.SelectNotificationEvents, "");

            var sessionModel = CreateSessionModel(inPerson, hybrid, online, all);
            mockSessionService.Setup(x => x.Get<OnboardingSessionModel>()).Returns(sessionModel);

            var result = controller.Get() as ViewResult;

            result.Should().NotBeNull();
            var viewModel = result.Model as NotificationsLocationsViewModel;
            viewModel.Should().NotBeNull();
            viewModel.IntroText.Should().Be(expectedIntroText);
        }

        private OnboardingSessionModel CreateSessionModel(bool inPerson, bool hybrid, bool online, bool all)
        {
            var sessionModel = new OnboardingSessionModel
            {
                EventTypes = new List<EventTypeModel>()
            };

            if (inPerson)
            {
                sessionModel.EventTypes.Add(new EventTypeModel { EventType = EventType.InPerson, IsSelected = true });
            }
            if (hybrid)
            {
                sessionModel.EventTypes.Add(new EventTypeModel { EventType = EventType.Hybrid, IsSelected = true });
            }
            if (online)
            {
                sessionModel.EventTypes.Add(new EventTypeModel { EventType = EventType.Online, IsSelected = true });
            }
            if (all)
            {
                sessionModel.EventTypes.Add(new EventTypeModel { EventType = EventType.All, IsSelected = true });
            }

            return sessionModel;
        }
    }
}
