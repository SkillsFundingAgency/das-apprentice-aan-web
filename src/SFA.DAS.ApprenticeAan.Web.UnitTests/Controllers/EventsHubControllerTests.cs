using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class EventsHubControllerTests
{
    private static readonly string AllNetworksUrl = Guid.NewGuid().ToString();

    private IActionResult _result = null!;
    private Mock<IOuterApiClient> _outerApiClientMock = null!;
    private EventsHubController _sut = null!;
    private CancellationToken _cancellationToken;

    [SetUp]
    public async Task WhenGetEventsHub()
    {
        var memberId = Guid.NewGuid();
        _cancellationToken = new();
        var fromDate = (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).ToString("yyyy-MM-dd");
        var toDate = (new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month))).ToString("yyyy-MM-dd");

        Fixture fixture = new();
        var attendances = fixture.Create<GetAttendancesResponse>();

        _outerApiClientMock = new();
        _outerApiClientMock.Setup(o => o.GetAttendances(memberId, fromDate, toDate, _cancellationToken)).ReturnsAsync(attendances);

        _sut = new(_outerApiClientMock.Object);
        _sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.NetworkEvents, AllNetworksUrl);
        _sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());

        _result = await _sut.Index(null, null, _cancellationToken);
    }

    [Test]
    public void ThenReturnsView()
        => _result.Should().BeOfType<ViewResult>();

    [Test]
    public void ThenRetrievesAttendances()
        => _outerApiClientMock.Verify(o => o.GetAttendances(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

    [Test]
    public void ThenSetsViewModel()
        => _result.As<ViewResult>().Model.Should().BeOfType<EventsHubViewModel>();

    [Test]
    public void InvalidValue_ThenThrowsArgumentOutOfRangeException()
    {
        Func<Task> action = () => _sut.Index(13, null, _cancellationToken);
        action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task OnlyMonthGiven_AssumesCurrentYear()
    {
        var result = await _sut.Index(DateTime.Today.Month, null, _cancellationToken);

        result.As<ViewResult>().Model.As<EventsHubViewModel>().Calendar.FirstDayOfCurrentMonth.Should().Be(new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1));
    }

    [Test]
    public async Task OnlyYearGiven_AssumesCurrentMonth()
    {
        var result = await _sut.Index(null, DateTime.Today.Year, _cancellationToken);

        result.As<ViewResult>().Model.As<EventsHubViewModel>().Calendar.FirstDayOfCurrentMonth.Should().Be(new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1));
    }
}
