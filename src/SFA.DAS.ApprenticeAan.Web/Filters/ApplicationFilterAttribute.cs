using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SFA.DAS.ApprenticeAan.Web.Filters;

[ExcludeFromCodeCoverage]
public abstract class ApplicationFilterAttribute : ActionFilterAttribute
{
    const string DefaultActionName = "Index";
    const string DefaultControllerName = "Home";
    const string OnboardingFilter = "Onboarding";

    protected readonly static RedirectToActionResult RedirectToHome = new(DefaultActionName, DefaultControllerName, null);

    protected static bool IsRequestForOnboardingAction(ControllerActionDescriptor action) => action.ControllerTypeInfo.FullName!.Contains(OnboardingFilter);
}
