using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Extensions;

public class DateTimeExtensionsTests
{
    [Test, AutoData]
    public void ToApiString_ReturnsFormattedDateOnlyString(DateTime date)
    {
        DateTimeExtensions.ToApiString(DateOnly.FromDateTime(date)).Should().Be(date.ToString("yyyy-MM-dd"));
    }

    [Test, AutoData]
    public void ToApiString_ReturnsFormattedDateTimeString(DateTime date)
    {
        date.ToApiString().Should().Be(date.ToString("yyyy-MM-dd"));
    }

    [Test, AutoData]
    public void ToScreenString_ReturnsFormattedDateTimeString(DateTime date)
    {
        date.ToScreenString().Should().Be(date.ToString("dd/MM/yyyy"));
    }

    [TestCase(DateTimeKind.Utc)]
    [TestCase(DateTimeKind.Unspecified)]
    public void UtcToLocalTime(DateTimeKind kind)
    {
        var date = new DateTime(2023, 5, 7, 13, 10, 0, kind);
        var actual = date.UtcToLocalTime();
        actual.Hour.Should().Be(14);
    }
}
