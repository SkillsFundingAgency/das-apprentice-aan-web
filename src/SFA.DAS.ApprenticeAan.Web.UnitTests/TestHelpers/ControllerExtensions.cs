using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers
{
    public static class ControllerExtensions
    {
        public static Mock<IUrlHelper> AddUrlHelperMock(this Controller controller)
        {
            var urlHelperMock = new Mock<IUrlHelper>();
            controller.Url = urlHelperMock.Object;
            return urlHelperMock;
        }

        public static Mock<IUrlHelper> AddUrlForRoute(this Mock<IUrlHelper> urlHelperMock, string routeName, string url = TestConstants.DefaultUrl)
        {
            urlHelperMock
               .Setup(m => m.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName!.Equals(routeName))))
               .Returns(url);
            return urlHelperMock;
        }
    }
}
