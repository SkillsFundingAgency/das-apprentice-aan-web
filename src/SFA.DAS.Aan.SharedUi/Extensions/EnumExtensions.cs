using System.ComponentModel;

namespace SFA.DAS.Aan.SharedUi.Extensions;

public static class EnumExtensions
{
    public static string? GetDescription(this Enum value)
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        var field = type.GetField(name!);
        if (Attribute.GetCustomAttribute(field!,
                typeof(DescriptionAttribute)) is DescriptionAttribute attr)
        {
            return attr.Description;
        }
        return value.ToString();
    }
}