using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class HomeControllerTests
{
    [Test]
    public void Index_ApprenticeIsMember_RedirectsToNetworkHub()
    {
        var memberId = Guid.NewGuid().ToString();
        var sessionServiceMock = new Mock<ISessionService>();
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.MemberId)).Returns(memberId.ToString());
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.MemberStatus)).Returns(Constants.MemberStatus.Live);


        var sut = new HomeController(sessionServiceMock.Object);

        var result = sut.Index();

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.NetworkHub);
    }

    [Test]
    public void Index_ApprenticeIsMember_RedirectsToOnboardingBeforeYouStart()
    {
        var sut = new HomeController(Mock.Of<ISessionService>());
        var result = sut.Index();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.BeforeYouStart);
    }
}