using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Web.Filters;

[ExcludeFromCodeCoverage]
public class RequiresExistingMemberAttribute : ApplicationFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor) return;

        if (BypassCheck(controllerActionDescriptor)) return;

        if (!IsValidRequest(context, controllerActionDescriptor))
        {
            context.Result = RedirectToHome;
        }
    }

    private bool BypassCheck(ControllerActionDescriptor controllerActionDescriptor)
    {
        var controllersToByPass = new[] { nameof(HomeController), nameof(LocationsController), nameof(AccessDeniedController) };

        return controllersToByPass.Contains(controllerActionDescriptor.ControllerTypeInfo.Name);
    }

    private static bool IsValidRequest(ActionExecutingContext context, ControllerActionDescriptor controllerActionDescriptor)
    {
        var isMember = context.HttpContext.User.GetAanMemberId() != Guid.Empty;

        var isRequestingOnboardingPage = IsRequestForOnboardingAction(controllerActionDescriptor);

        return (isMember && !isRequestingOnboardingPage) || (!isMember && isRequestingOnboardingPage);
    }
}
