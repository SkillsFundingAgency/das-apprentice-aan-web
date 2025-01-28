using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses.Shared;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Orchestrators.Shared;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.NotificationLocationDisambiguationControllerTests
{
    [TestFixture]
    public class NotificationLocationDisambiguationControllerGetTests
    {
        [Test, MoqAutoData]
        public async Task Get_WhenCalled_ReturnsViewWithViewModel(
            [Frozen] Mock<ISessionService> mockSessionService,
            [Frozen] Mock<IOuterApiClient> mockOuterApiClient,
            [Frozen] Mock<INotificationLocationDisambiguationOrchestrator> mockOrchestrator,
            string employerAccountId,
            int radius,
            string location,
            List<GetNotificationsLocationSearchApiResponse.Location> locations,
            NotificationLocationDisambiguationViewModel orchestratorViewModel,
            string notificationsLocationsUrl,
            OnboardingSessionModel sessionModel,
            [Greedy] NotificationLocationDisambiguationController controller)
        {
            mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

            orchestratorViewModel.Location = location;
            orchestratorViewModel.Locations = locations
                .Select(x => new LocationModel { Name = x.Name, LocationId = x.Name })
                .Take(10)
                .ToList();

            mockOrchestrator
                .Setup(o => o.GetViewModel<NotificationLocationDisambiguationViewModel>(radius, location))
                .ReturnsAsync(orchestratorViewModel);

            controller.AddUrlHelperMock()
                .AddUrlForRoute(RouteNames.Onboarding.NotificationsLocations, notificationsLocationsUrl);

            var result = await controller.Get(radius, location) as ViewResult;

            result.Should().NotBeNull();
            result!.ViewName.Should().Be(NotificationLocationDisambiguationController.ViewPath);

            var viewModel = result.Model as NotificationLocationDisambiguationViewModel;
            viewModel.Should().NotBeNull();
            viewModel!.BackLink.Should().Be(notificationsLocationsUrl);
            viewModel.Location.Should().Be(location);
            viewModel.Locations.Should().HaveCountLessOrEqualTo(10);
        }
    }
}
