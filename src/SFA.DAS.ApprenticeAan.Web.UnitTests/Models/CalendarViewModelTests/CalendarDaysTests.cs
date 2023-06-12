using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models.CalendarViewModelTests;

public class CalendarDaysTests
{

    [TestCase(2024, 1, CalendarViewModel.TotalCalendarDaysNormal)]
    [TestCase(2024, 2, CalendarViewModel.TotalCalendarDaysNormal)]
    [TestCase(2024, 9, CalendarViewModel.TotalCalendarDaysExtended)]
    [TestCase(2024, 12, CalendarViewModel.TotalCalendarDaysExtended)]
    public void ThenHasCorrectNoOfItems(int year, int month, int totalDaysToRender)
    {
        CalendarViewModel sut = new(month, year, Mock.Of<IUrlHelper>(), DateOnly.FromDateTime(DateTime.Today));
        sut.CalendarItems.Should().HaveCount(totalDaysToRender);
    }

    [TestCase(2024, 1, 0)]  //Monday
    [TestCase(2024, 10, 1)] //T
    [TestCase(2024, 5, 2)]  //W
    [TestCase(2024, 2, 3)]  //T
    [TestCase(2024, 11, 4)]  //F
    [TestCase(2024, 6, 5)]  //S
    [TestCase(2024, 12, 6)]  //Sunday
    public void ThenStartsRenderingOnTheFirstDayOfTheMonth(int year, int month, int expectedStartIndex)
    {
        CalendarViewModel sut = new(month, year, Mock.Of<IUrlHelper>(), DateOnly.FromDateTime(DateTime.Today));
        sut.CalendarItems[expectedStartIndex].Day.Should().Be(new DateOnly(year, month, 1));
        sut.CalendarItems.Where(r => r.Index < expectedStartIndex).All(d => d.Day == null).Should().BeTrue();
    }

    [TestCase(2024, 1, 31)]
    [TestCase(2024, 2, 29)]
    [TestCase(2024, 4, 30)]
    [TestCase(2024, 12, 31)]
    public void ThenStopsRenderingAfterLastDayOfTheMonth(int year, int month, int expectedDaysToRender)
    {
        CalendarViewModel sut = new(month, year, Mock.Of<IUrlHelper>(), DateOnly.FromDateTime(DateTime.Today));
        sut.CalendarItems.Where(r => r.Day != null).Should().HaveCount(expectedDaysToRender);
    }

    [TestCase(2024, 1, 1, true)]
    [TestCase(2024, 1, -1, false)]
    public void ThenSetsIsTodayWhereApplicable(int year, int month, int addDaysToToday, bool expectIsToday)
    {
        var date = new DateOnly(year, month, 1);
        var today = date.AddDays(addDaysToToday);
        CalendarViewModel sut = new(month, year, Mock.Of<IUrlHelper>(), today);
        sut.CalendarItems.Any(c => c.IsToday).Should().Be(expectIsToday);
    }
}
