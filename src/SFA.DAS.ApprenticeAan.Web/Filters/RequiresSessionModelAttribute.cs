using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.Filters;

[ExcludeFromCodeCoverage]
public class RequiresSessionModelAttribute : ActionFilterAttribute
{
    public const string ActionName = "Index";
    public const string ControllerName = "Home";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (BypassCheck(context)) return;

        ISessionService sessionService = context.HttpContext.RequestServices.GetService<ISessionService>()!;

        OnboardingSessionModel sessionModel = sessionService.Get<OnboardingSessionModel>();

        if (sessionModel == null || !sessionModel.IsValid)
        {
            context.Result = new RedirectToActionResult(ActionName, ControllerName, null);
        }
    }

    private bool BypassCheck(ActionExecutingContext context)
    {
        var controllersToByPass = new[] { nameof(BeforeYouStartController), nameof(TermsAndConditionsController), nameof(LineManagerController) };

        if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return true;

        if (!controllerActionDescriptor.ControllerTypeInfo.FullName!.Contains("Onboarding")) return true;

        if (controllersToByPass.Any(c => c == controllerActionDescriptor.ControllerTypeInfo.Name)) return true;

        return false;
    }
}
