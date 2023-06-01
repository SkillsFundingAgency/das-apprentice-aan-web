using FluentAssertions;
using SFA.DAS.ApprenticeAan.Web.HtmlHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.HtmlHelpers;

[TestFixture]
public class DateTimeHelperTests
{
    [TestCase(null, null)]
    [TestCase("2022-03-04 08:00:05", "04/03/2022")]
    [TestCase("31 Mar 2025", "31/03/2025")]
    public void ToScreenFormat_ReturnsExpectedFormat(DateTime? date, string? expected)
    {
        var actual = DateTimeHelper.ToScreenFormat(date);
        actual.Should().Be(expected);
    }

    [TestCase(null, null)]
    [TestCase("2022-03-04 08:00:05", "2022-03-04")]
    [TestCase("31 Mar 2025", "2025-03-31")]
    public void ToUrlFormat_ReturnsExpectedFormat(DateTime? date, string? expected)
    {
        var actual = DateTimeHelper.ToUrlFormat(date);
        actual.Should().Be(expected);
    }
}