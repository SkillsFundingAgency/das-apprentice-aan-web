using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

public class EventsHubViewModelTests
{
    [Test]
    public void Constructor_SetsMonth()
    {
        EventsHubViewModel sut = new(10, 1, Mock.Of<IUrlHelper>());

        sut.Calendar.FirstDayOfCurrentMonth.Month.Should().Be(10);
    }

    [Test]
    public void Constructor_SetsYear()
    {
        EventsHubViewModel sut = new(10, 2001, Mock.Of<IUrlHelper>());

        sut.Calendar.FirstDayOfCurrentMonth.Year.Should().Be(2001);
    }

    [Test]
    public void Constructor_SetsAllNetworksUrl()
    {
        const string route = "route";
        Mock<IUrlHelper> urlHelperMock = new();
        urlHelperMock.Setup(h => h.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName == RouteNames.NetworkEvents))).Returns(route);
        EventsHubViewModel sut = new(10, 1, urlHelperMock.Object);

        sut.AllNetworksUrl.Should().Be(route);
    }

}
