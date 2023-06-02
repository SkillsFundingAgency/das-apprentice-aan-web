using FluentAssertions;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models;

[TestFixture]
public class NetworkEventsViewModelTests
{
    [Test]
    [MoqAutoData]
    public void NetworkEventsViewModel_From_GetCalendarEventsQueryResult(GetCalendarEventsQueryResult result)
    {
        var sut = (NetworkEventsViewModel)result;

        Assert.Multiple(() =>
        {
            Assert.That(sut, Is.Not.Null);
            Assert.That(sut.Page, Is.EqualTo(result.Page));
            Assert.That(sut.PageSize, Is.EqualTo(result.PageSize));
            Assert.That(sut.TotalPages, Is.EqualTo(result.TotalPages));
            Assert.That(sut.TotalCount, Is.EqualTo(result.TotalCount));
            Assert.That(sut.CalendarEvents, Is.EquivalentTo(result.CalendarEvents));
        });
    }

    [TestCase(null, null, false)]
    [TestCase("2023-05-26", null, true)]
    [TestCase(null, "2024-01-01", true)]
    [TestCase("2023-05-26", "2024-01-01", true)]
    public void ShowFilterOptions_ReturningExpectedValueFromParameters(DateTime? startDate, DateTime? endDate, bool expected)
    {
        var model = new NetworkEventsViewModel
        {
            StartDate = startDate,
            EndDate = endDate
        };
        var actual = model.ShowFilterOptions;
        actual.Should().Be(expected);
    }

    [TestCase(NetworkEventsViewModel.FilterFields.StartDate, "2023-05-31", "2024-01-01", "?endDate=2024-01-01")]
    [TestCase(NetworkEventsViewModel.FilterFields.EndDate, "2023-05-31", "2024-01-01", "?startDate=2023-05-31")]
    [TestCase(NetworkEventsViewModel.FilterFields.StartDate, null, null, "")]
    [TestCase(NetworkEventsViewModel.FilterFields.EndDate, null, null, "")]
    [TestCase(NetworkEventsViewModel.FilterFields.StartDate, "2023-05-31", null, "")]
    [TestCase(NetworkEventsViewModel.FilterFields.StartDate, null, "2024-01-01", "?endDate=2024-01-01")]
    [TestCase(NetworkEventsViewModel.FilterFields.EndDate, null, "2024-01-01", "")]
    [TestCase(NetworkEventsViewModel.FilterFields.EndDate, "2023-05-31", "2024-01-01", "?startDate=2023-05-31")]
    public void QueryStringWhenFilterRemoved(NetworkEventsViewModel.FilterFields filterToRemove, DateTime? startDate, DateTime? endDate, string expected)
    {
        var model = new NetworkEventsViewModel
        {
            StartDate = startDate,
            EndDate = endDate
        };
        var actual = model.BuildQueryString(filterToRemove);
        actual.Should().Be(expected);
    }
}