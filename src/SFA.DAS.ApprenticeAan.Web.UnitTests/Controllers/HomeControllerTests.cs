using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
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
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.MemberId)).Returns(memberId.ToString());
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.Status)).Returns(MemberStatus.Live.ToString());


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

    [TestCase(MemberStatus.Withdrawn)]
    [TestCase(MemberStatus.Deleted)]
    public void Index_MemberIsNotLive_RedirectsToRejoinTheNetwork(MemberStatus memberStatus)
    {
        var memberId = Guid.NewGuid().ToString();
        var sessionServiceMock = new Mock<ISessionService>();
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.MemberId)).Returns(memberId.ToString());
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.Status)).Returns(memberStatus.ToString());

        var sut = new HomeController(sessionServiceMock.Object);

        var result = sut.Index();

        result.As<RedirectToRouteResult>().RouteName.Should().Be(SharedRouteNames.RejoinTheNetwork);
    }
}