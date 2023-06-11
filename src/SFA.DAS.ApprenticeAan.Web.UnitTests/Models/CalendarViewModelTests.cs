using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

public class CalendarViewModelTests
{
    private const int Month = 1;
    private const int Year = 2023;
    private const string EventsHubRoute = "EventsHubRoute";

    private Mock<IUrlHelper> _urlHelperMock = null!;
    private CalendarViewModel _sut = null!;

    [SetUp]
    public void Initialize()
    {
        _urlHelperMock = new();
        _urlHelperMock.Setup(h => h.RouteUrl(It.Is<UrlRouteContext>(c
            => c.RouteName == RouteNames.NetworkEvents
            ))).Returns(EventsHubRoute);
        _sut = new(Month, Year, _urlHelperMock.Object);
    }

    [Test]
    public void ThenSetsCurrentMonth()
        => _sut.CurrentMonth.Should().Be(Month);

    [Test]
    public void ThenSetCurrentYear()
        => _sut.CurrentYear.Should().Be(Year);

    [Test]
    public void ThenFirstDayOfCurrentMonthIsCorrect()
        => _sut.FirstDayOfCurrentMonth.Should().Be(DateOnly.FromDateTime(new DateTime(Year, Month, 1)));

    [Test]
    public void ThenPreviousMonthLinkIsSet()
        => _sut.PreviousMonthLink.Should().NotBeEmpty();

    [Test]
    public void ThenNextMonthLinkIsSet()
        => _sut.NextMonthLink.Should().NotBeEmpty();

}
