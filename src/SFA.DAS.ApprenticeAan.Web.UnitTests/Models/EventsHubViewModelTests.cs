using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

public class EventsHubViewModelTests
{
    private const int Year = 2024;
    private const int Month = 1;
    private const string NetworkEventsRoute = "network-events";
    private const string NetworkEventDetailsRoute = "event-details";
    private readonly static DateOnly _date = new(Year, Month, 1);

    [Test]
    public void Constructor_SetsFirstDayOfTheCurrentMonth()
    {
        EventsHubViewModel sut = new(_date, Mock.Of<IUrlHelper>(), new());

        sut.Calendar.FirstDayOfCurrentMonth.Should().Be(_date);
    }

    [Test]
    public void Constructor_SetsAllNetworksUrl()
    {
        Mock<IUrlHelper> urlHelperMock = new();
        urlHelperMock.Setup(h => h.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName == RouteNames.NetworkEvents))).Returns(NetworkEventsRoute);

        EventsHubViewModel sut = new(_date, urlHelperMock.Object, new());

        sut.AllNetworksUrl.Should().Be(NetworkEventsRoute);
    }

    [Test]
    public void Constructor_BuildsCalendarViewModel()
    {
        var startDates = new[] { new DateTime(Year, Month, 1), new DateTime(Year, Month, 1), new DateTime(Year, Month, 2) };
        Fixture fixture = new();
        var attendances = fixture.Build<Attendance>().WithValues(a => a.EventStartDate, startDates).CreateMany().ToList();
        Mock<IUrlHelper> urlHelperMock = new();
        urlHelperMock.Setup(h => h.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName == RouteNames.NetworkEvents))).Returns(NetworkEventsRoute);
        urlHelperMock.Setup(h => h.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName == RouteNames.NetworkEventDetails))).Returns(NetworkEventDetailsRoute);

        EventsHubViewModel sut = new(_date, urlHelperMock.Object, attendances);

        sut.Calendar.CalendarItems.SelectMany(c => c.Appointments).Should().HaveCount(attendances.Count);
    }
}
