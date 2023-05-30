using FluentAssertions;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Extensions;

[TestFixture]
public class EnumExtensionsTests
{
    [TestCase(EventFormat.InPerson, "In person")]
    [TestCase(EventFormat.Hybrid, "Hybrid")]
    [TestCase(EventFormat.Online, "Online")]
    public void ToDescription_ReturnsExpectedFormat(EventFormat eventFormat, string expected)
    {
        var actual = eventFormat.GetDescription();
        actual.Should().Be(expected);
    }

    [TestCase(EventFormat.InPerson, "inperson")]
    [TestCase(EventFormat.Hybrid, "hybrid")]
    [TestCase(EventFormat.Online, "online")]
    public void ToLower_ReturnsExpectedFormat(EventFormat eventFormat, string expected)
    {
        var actual = eventFormat.ToLower();
        actual.Should().Be(expected);
    }
}
