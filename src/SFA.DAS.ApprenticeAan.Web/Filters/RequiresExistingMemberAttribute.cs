using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ApprenticeAan.Web.AppStart;
using SFA.DAS.ApprenticeAan.Web.Controllers;

namespace SFA.DAS.ApprenticeAan.Web.Filters;

[ExcludeFromCodeCoverage]
public class RequiresExistingMemberAttribute : ApplicationFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;

        if (BypassCheck(controllerActionDescriptor)) return;

        if (IsInvalidRequest(context, controllerActionDescriptor))
        {
            context.Result = RedirectToHome;
        }
    }

    private bool BypassCheck(ControllerActionDescriptor controllerActionDescriptor)
    {
        var controllersToByPass = new[] { nameof(HomeController), nameof(LocationsController) };

        return controllersToByPass.Any(c => c == controllerActionDescriptor.ControllerTypeInfo.Name);
    }

    private static bool IsInvalidRequest(ActionExecutingContext context, ControllerActionDescriptor controllerActionDescriptor)
    {
        var isMember = context.HttpContext.User.GetAanMemberId() != null;

        var isRequestingOnboardingPage = IsRequestForOnboardingAction(controllerActionDescriptor);

        return (isMember && isRequestingOnboardingPage) || (!isMember && !isRequestingOnboardingPage);
    }
}
