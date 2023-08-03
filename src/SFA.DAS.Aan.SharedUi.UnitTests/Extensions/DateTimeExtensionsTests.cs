using FluentAssertions;
using SFA.DAS.Aan.SharedUi.Extensions;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Extensions;

public class DateTimeExtensionsTests
{
    [TestCase(DateTimeKind.Utc)]
    [TestCase(DateTimeKind.Unspecified)]
    public void UtcToLocalTime(DateTimeKind kind)
    {
        var date = new DateTime(2023, 5, 7, 13, 10, 0, kind);
        var actual = date.SharedUtcToLocalTime();
        actual.Hour.Should().Be(14);
    }
}
