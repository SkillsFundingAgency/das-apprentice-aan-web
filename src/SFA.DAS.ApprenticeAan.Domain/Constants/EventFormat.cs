using System.ComponentModel;

namespace SFA.DAS.ApprenticeAan.Domain.Constants;

public enum EventFormat
{
    [Description("In person")]
    InPerson,
    [Description("Online")]
    Online,
    [Description("Hybrid")]
    Hybrid
}