using SFA.DAS.Aan.SharedUi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Aan.SharedUi.UnitTests.Services;
public class StringFormatterTests
{
    [Test]
    public void TrimValue_NullableString_ReturnNull()
    {
        // Act
        string? sut = StringFormatter.TrimValue(null);

        // Assert
        Assert.That(sut, Is.Null);
    }

    [Test]
    [MoqInlineAutoData("test")]
    [MoqInlineAutoData(" test")]
    [MoqInlineAutoData("test ")]
    [MoqInlineAutoData(" test ")]
    public void TrimValue_NonNullableString_ReturnsExpectedString(string? value)
    {
        // Act
        string? sut = StringFormatter.TrimValue(value);

        // Assert
        Assert.That(sut, Is.EqualTo(value?.Trim()));
    }
}
