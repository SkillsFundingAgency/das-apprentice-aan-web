using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class HomeControllerTests
{
    [Test]
    public void Index_ApprenticeIsMember_RedirectsToNetworkHub()
    {
        var sut = new HomeController();
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());

        var result = sut.Index();

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.NetworkHub);
    }

    [Test]
    public void Index_ApprenticeIsMember_RedirectsToOnboardingBeforeYouStart()
    {
        var sut = new HomeController();
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.Empty.ToString());

        var result = sut.Index();

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.BeforeYouStart);
    }
}