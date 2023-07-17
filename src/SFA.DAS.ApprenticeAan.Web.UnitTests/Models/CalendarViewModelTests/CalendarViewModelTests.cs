using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models.CalendarViewModelTests;

public class CalendarViewModelTests
{
    private static readonly DateOnly _date = new DateOnly(2023, 6, 1);
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
        _sut = new(_date, DateOnly.FromDateTime(DateTime.Today), Enumerable.Empty<Appointment>());
    }

    [Test]
    public void ThenFirstDayOfCurrentMonthIsCorrect()
        => _sut.FirstDayOfCurrentMonth.Should().Be(_date);

    [Test]
    public void ThenPreviousMonthLinkIsSet()
        => _sut.PreviousMonthLink.Should().NotBeEmpty();

    [Test]
    public void ThenNextMonthLinkIsSet()
        => _sut.NextMonthLink.Should().NotBeEmpty();
}
