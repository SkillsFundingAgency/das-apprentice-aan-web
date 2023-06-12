using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Extensions;

public class DateTimeExtensionsTests
{
    [Test, AutoData]
    public void ToApiString_ReturnsFormattedDateString(DateTime date)
    {
        DateTimeExtensions.ToApiString(DateOnly.FromDateTime(date)).Should().Be(date.ToString("yyyy-MM-dd"));
    }
}
