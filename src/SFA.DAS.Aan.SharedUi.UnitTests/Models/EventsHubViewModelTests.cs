using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.UnitTests.TestHelpers;

namespace SFA.DAS.Aan.Web.UnitTests.Models;

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
        EventsHubViewModel sut = new(_date, Mock.Of<IUrlHelper>(), new(), () => { return NetworkEventsRoute; });

        sut.Calendar.FirstDayOfCurrentMonth.Should().Be(_date);
    }

    [Test]
    public void Constructor_SetsAllNetworksUrl()
    {
        Mock<IUrlHelper> urlHelperMock = new();
        urlHelperMock.Setup(h => h.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName == SharedRouteNames.NetworkEvents))).Returns(NetworkEventsRoute);

        EventsHubViewModel sut = new(_date, urlHelperMock.Object, new(), () => { return NetworkEventsRoute; });

        sut.AllNetworksUrl.Should().Be(NetworkEventsRoute);
    }

    [Test]
    public void Constructor_BuildsCalendarViewModel()
    {
        var startDates = new[] { new DateOnly(Year, Month, 1), new DateOnly(Year, Month, 1), new DateOnly(Year, Month, 2) };
        Fixture fixture = new();
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
        var appointments = fixture.Build<Appointment>().WithValues(a => a.Date, startDates).CreateMany().ToList();
        Mock<IUrlHelper> urlHelperMock = new();
        urlHelperMock.Setup(h => h.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName == SharedRouteNames.NetworkEvents))).Returns(NetworkEventsRoute);
        urlHelperMock.Setup(h => h.RouteUrl(It.Is<UrlRouteContext>(c => c.RouteName == SharedRouteNames.NetworkEventDetails))).Returns(NetworkEventDetailsRoute);

        EventsHubViewModel sut = new(_date, urlHelperMock.Object, appointments, () => { return NetworkEventsRoute; });

        sut.Calendar.CalendarItems.SelectMany(c => c.Appointments).Should().HaveCount(appointments.Count);
    }
}
