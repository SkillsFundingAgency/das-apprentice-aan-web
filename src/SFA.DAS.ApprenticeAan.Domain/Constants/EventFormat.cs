
using System.ComponentModel;

namespace SFA.DAS.ApprenticeAan.Domain.Constants;

//[ExcludeFromCodeCoverage]
// public static class EventFormat
// {
//     public const string Hybrid = "Hybrid";
//     public const string InPerson = "In person";
//     public const string Online = "Online";
// }
public enum EventFormat
{
    [Description("In person")]
    InPerson,
    Online,
    Hybrid
}