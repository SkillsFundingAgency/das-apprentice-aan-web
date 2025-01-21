using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Constant;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.SelectNotificationsControllerTests
{
    [TestFixture]
    public class SelectNotificationsControllerGetTests
    {
        [Test, MoqAutoData]
        public void Get_WhenCalled_ReturnsViewResultWithViewModel(
            [Frozen] Mock<IValidator<SelectNotificationsSubmitModel>> validator,
            [Frozen] Mock<ISessionService> mockSessionService,
            string checkYourAnswersUrl,
            string receiveNotificationsUrl,
            OnboardingSessionModel sessionModel,
            [Greedy] SelectNotificationsController controller)
        {
            sessionModel.EventTypes = new List<EventTypeModel>
            {
                new() { EventType = EventType.Hybrid, IsSelected = false, Ordering = 1 },
                new() { EventType = EventType.InPerson, IsSelected = false, Ordering = 2 }
            };

            mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

            controller.AddUrlHelperMock()
                      .AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers, checkYourAnswersUrl)
                      .AddUrlForRoute(RouteNames.Onboarding.ReceiveNotifications, receiveNotificationsUrl);

            var result = controller.Get() as ViewResult;

            result.Should().NotBeNull();
            result!.ViewName.Should().Be(SelectNotificationsController.ViewPath);

            var viewModel = result.Model as SelectNotificationsViewModel;
            viewModel.Should().NotBeNull();
            viewModel!.EventTypes.Should().BeEquivalentTo(sessionModel.EventTypes);
            viewModel.BackLink.Should().Be(sessionModel.HasSeenPreview ? checkYourAnswersUrl : receiveNotificationsUrl);
        }
    }
}
