using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.ConfirmDetailsControllerTests
{
    public class ConfirmDetailsControllerTests
    {
        [Theory, MoqAutoData]
        public void Index_Get_ReturnsViewWithViewModel(
            [Frozen] Mock<ISessionService> sessionServiceMock,
            [Greedy] ConfirmDetailsController controller,
            OnboardingSessionModel sessionModel,
            string backLink)
        {
            sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>())
                .Returns(sessionModel);

            controller.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.RegionalNetwork, backLink);

            var expectedViewModel = new ConfirmDetailsViewModel
            {
                BackLink = backLink,
                FullName = sessionModel.ApprenticeDetails.Name,
                Email = sessionModel.ApprenticeDetails.Email,
                ApprenticeshipSector = sessionModel.MyApprenticeship.TrainingCourse?.Sector,
                ApprenticeshipProgram = sessionModel.MyApprenticeship.TrainingCourse?.Name,
                ApprenticeshipLevel = sessionModel.MyApprenticeship.TrainingCourse?.Level,
            };

            var result = controller.Index() as ViewResult;

            result.Should().NotBeNull();
            result.ViewName.Should().Be(ConfirmDetailsController.ViewPath);
            result.Model.Should().BeOfType<ConfirmDetailsViewModel>()
                .Which.Should().BeEquivalentTo(expectedViewModel);
        }

        [Theory, MoqAutoData]
        public void IndexPost_Post_RedirectsToEmployerSearch(
            [Greedy] ConfirmDetailsController controller)
        {
            var result = controller.IndexPost() as RedirectToRouteResult;

            result.Should().NotBeNull();
            result.RouteName.Should().Be(RouteNames.Onboarding.EmployerSearch);
        }
    }
}
