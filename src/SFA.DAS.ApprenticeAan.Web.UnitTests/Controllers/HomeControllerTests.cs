using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticePortal.Authentication;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        [Test, MoqAutoData]
        public async Task Index_ApprenticeIsMember_RedirectsToNetworkHub()
        {
            AuthenticatedUser user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerified;
            Mock<IApprenticesService> apprenticesServiceMock = new();
            apprenticesServiceMock.Setup(a => a.GetApprentice(user.ApprenticeId)).ReturnsAsync(new Domain.OuterApi.Responses.Apprentice());
            var controller = new HomeController(apprenticesServiceMock.Object, user);

            var result = await controller.Index();

            result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.NetworkHub);
        }

        [Test, MoqAutoData]
        public async Task Index_ApprenticeIsMember_RedirectsToOnboardingBeforeYouStart()
        {
            AuthenticatedUser user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerified;
            Mock<IApprenticesService> apprenticesServiceMock = new();
            apprenticesServiceMock.Setup(a => a.GetApprentice(user.ApprenticeId)).ReturnsAsync(() => null);
            var controller = new HomeController(apprenticesServiceMock.Object, user);

            var result = await controller.Index();

            result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.BeforeYouStart);
        }
    }
}