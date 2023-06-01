using FluentAssertions;
using SFA.DAS.ApprenticeAan.Domain.Extensions;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Extensions;

[TestFixture]
public class EnumExtensionsTests
{
    [TestCase(TestEnum.Item1, "Item 1")]
    [TestCase(TestEnum.Item2, "Item 2")]
    [TestCase(TestEnum.Item3, "Item3")]
    public void ToDescription_ReturnsExpectedFormat(TestEnum enumTested, string expected)
    {
        var actual = enumTested.GetDescription();
        actual.Should().Be(expected);
    }

    [TestCase(TestEnum.Item1, "item1")]
    [TestCase(TestEnum.Item2, "item2")]
    [TestCase(TestEnum.Item3, "item3")]
    public void ToLower_ReturnsExpectedFormat(TestEnum enumTested, string expected)
    {
        var actual = enumTested.ToLower();
        actual.Should().Be(expected);
    }
}


public enum TestEnum
{
    [Description("Item 1")]
    Item1,
    [Description("Item 2")]
    Item2,
    Item3
}