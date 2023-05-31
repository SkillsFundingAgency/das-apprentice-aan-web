using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

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

    public static void AddContextWithClaim(this Controller controller, string claimType, string claimValue)
    {
        var httpContext = new Mock<HttpContext>();
        Claim claim = new(claimType, claimValue);
        httpContext.Setup(m => m.User.FindFirst(claimType)).Returns(claim);
        controller.ControllerContext = new(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));
    }
}
