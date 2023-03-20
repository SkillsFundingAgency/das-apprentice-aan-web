using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Web.Models.Onboarding;

[ExcludeFromCodeCoverage]
public class NameOfEmployerViewModel : NameOfEmployerSubmitModel, IBackLink
{
    public string BackLink { get; set; } = null!;
}

[ExcludeFromCodeCoverage]
public class NameOfEmployerSubmitModel
{
    public string? NameOfEmployer { get; set; }
}