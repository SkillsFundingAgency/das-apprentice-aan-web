using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        [Test]
        public void Index_ReturnsPage()
        {
            var controller = new HomeController();

            var result = controller.Index();

            result.As<RedirectToRouteResult>().Should().NotBeNull();
            result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.BeforeYouStart);
        }
    }
}