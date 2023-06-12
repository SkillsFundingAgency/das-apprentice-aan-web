using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

public class EventsHubViewModelTests
{
    private readonly static DateOnly _date = new(2024, 1, 1);

    [Test]
    public void Constructor_SetsFirstDayOfTheCurrentMonth()
    {
        EventsHubViewModel sut = new(_date, Mock.Of<IUrlHelper>(), new());

        sut.Calendar.FirstDayOfCurrentMonth.Should().Be(_date);
    }

    [Test]
    public void Constructor_SetsAllNetworksUrl()
    {
        const string route = "route";
        Mock<IUrlHelper> urlHelperMock = new();
        urlHelperMock.Setup(h => h.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName == RouteNames.NetworkEvents))).Returns(route);
        EventsHubViewModel sut = new(_date, urlHelperMock.Object, new());

        sut.AllNetworksUrl.Should().Be(route);
    }

}
