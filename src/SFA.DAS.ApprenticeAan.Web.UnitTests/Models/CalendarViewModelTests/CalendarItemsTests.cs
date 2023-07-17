using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Models.CalendarViewModelTests;

public class CalendarItemsTests
{
    public const int TestYear = 2024;

    [Test]
    public void ThenHasCorrectNoOfItems()
    {
        var date = new DateOnly(2023, 6, 1);
        CalendarViewModel sut = new(date, DateOnly.FromDateTime(DateTime.Today), Enumerable.Empty<Appointment>());
        sut.CalendarItems.Should().HaveCount(CalendarViewModel.TotalCalendarDaysNormal);
    }

    [TestCase(1, CalendarViewModel.TotalCalendarDaysNormal)]
    [TestCase(2, CalendarViewModel.TotalCalendarDaysNormal)]
    [TestCase(9, CalendarViewModel.TotalCalendarDaysExtended)]
    [TestCase(12, CalendarViewModel.TotalCalendarDaysExtended)]
    public void ThenHasCorrectNoOfItems(int month, int totalDaysToRender)
    {
        var date = new DateOnly(TestYear, month, 1);
        CalendarViewModel sut = new(date, DateOnly.FromDateTime(DateTime.Today), Enumerable.Empty<Appointment>());
        sut.CalendarItems.Should().HaveCount(totalDaysToRender);
    }

    [TestCase(1, 0)]  //Monday
    [TestCase(10, 1)] //T
    [TestCase(5, 2)]  //W
    [TestCase(2, 3)]  //T
    [TestCase(11, 4)]  //F
    [TestCase(6, 5)]  //S
    [TestCase(12, 6)]  //Sunday
    public void ThenStartsRenderingOnTheFirstDayOfTheMonth(int month, int expectedStartIndex)
    {
        var date = new DateOnly(TestYear, month, 1);
        CalendarViewModel sut = new(date, DateOnly.FromDateTime(DateTime.Today), Enumerable.Empty<Appointment>());
        sut.CalendarItems[expectedStartIndex].Day.Should().Be(date);
        sut.CalendarItems.Where(r => r.Index < expectedStartIndex).All(d => d.Day == null).Should().BeTrue();
    }

    [TestCase(1, 31)]
    [TestCase(2, 29)]
    [TestCase(4, 30)]
    [TestCase(12, 31)]
    public void ThenStopsRenderingAfterLastDayOfTheMonth(int month, int expectedDaysToRender)
    {
        var date = new DateOnly(TestYear, month, 1);
        CalendarViewModel sut = new(date, DateOnly.FromDateTime(DateTime.Today), Enumerable.Empty<Appointment>());
        sut.CalendarItems.Where(r => r.Day != null).Should().HaveCount(expectedDaysToRender);
    }

    [TestCase(1, 1, true)]
    [TestCase(1, -1, false)]
    public void ThenSetsIsTodayWhereApplicable(int month, int addDaysToToday, bool expectIsToday)
    {
        var date = new DateOnly(TestYear, month, 1);
        var today = date.AddDays(addDaysToToday);
        CalendarViewModel sut = new(date, today, Enumerable.Empty<Appointment>());
        sut.CalendarItems.Any(c => c.IsToday).Should().Be(expectIsToday);
    }

    [Test]
    public void ThenPopulatesAppointments()
    {
        var date = new DateOnly(2024, 1, 1);

        var appointments = new[]
        {
            new Appointment(Guid.NewGuid().ToString(), string.Empty, date.AddDays(10), string.Empty),
            new Appointment(Guid.NewGuid().ToString(), string.Empty, date.AddDays(10), string.Empty),
            new Appointment(Guid.NewGuid().ToString(), string.Empty, date.AddDays(20), string.Empty),
            new Appointment(Guid.NewGuid().ToString(), string.Empty, date.AddDays(30), string.Empty),
        };

        CalendarViewModel sut = new(date, date.AddDays(2), appointments);

        sut.CalendarItems.Single(c => c.Day?.Day == 11).Appointments.Should().HaveCount(2);
        sut.CalendarItems.Single(c => c.Day?.Day == 21).Appointments.Should().HaveCount(1);
        sut.CalendarItems.Single(c => c.Day?.Day == 31).Appointments.Should().HaveCount(1);
    }
}
