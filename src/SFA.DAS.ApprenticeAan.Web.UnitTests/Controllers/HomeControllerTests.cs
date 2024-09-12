using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticePortal.Authentication;
using SFA.DAS.GovUK.Auth.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class HomeControllerTests
{
    [Test]
    public async Task Index_ApprenticeIsMember_RedirectsToNetworkHub()
    {
        var memberId = Guid.NewGuid().ToString();
        var sessionServiceMock = new Mock<ISessionService>();
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.MemberId)).Returns(memberId.ToString());
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.Status)).Returns(MemberStatus.Live.ToString());


        var sut = new HomeController(
            sessionServiceMock.Object,
            Mock.Of<ApplicationConfiguration>(), 
            Mock.Of<IOidcService>(), 
            Mock.Of<IApprenticeAccountProvider>());

        var result = await sut.Index();

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.NetworkHub);
    }

    [Test]
    public async Task Index_ApprenticeIsMember_RedirectsToOnboardingBeforeYouStart()
    {
        var sut = new HomeController(
            Mock.Of<ISessionService>(), 
            Mock.Of<ApplicationConfiguration>(), 
            Mock.Of<IOidcService>(), 
            Mock.Of<IApprenticeAccountProvider>()
            );
        var result = await sut.Index();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.BeforeYouStart);
    }

    [TestCase(MemberStatus.Withdrawn, SharedRouteNames.RejoinTheNetwork)]
    [TestCase(MemberStatus.Deleted, SharedRouteNames.RejoinTheNetwork)]
    [TestCase(MemberStatus.Removed, SharedRouteNames.RemovedShutter)]
    public async Task Index_MemberIsNotLive_RedirectsToRejoinTheNetwork(MemberStatus memberStatus, string routeName)
    {
        var memberId = Guid.NewGuid().ToString();
        var sessionServiceMock = new Mock<ISessionService>();
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.MemberId)).Returns(memberId.ToString());
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.Status)).Returns(memberStatus.ToString());

        var sut = new HomeController(sessionServiceMock.Object,
            Mock.Of<ApplicationConfiguration>(), 
            Mock.Of<IOidcService>(), 
            Mock.Of<IApprenticeAccountProvider>());

        var result = await sut.Index();

        result.As<RedirectToRouteResult>().RouteName.Should().Be(routeName);
    }
}