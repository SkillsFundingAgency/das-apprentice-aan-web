using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ApprenticeAan.Web.Extensions;

namespace SFA.DAS.ApprenticeAan.Web.Authentication;

public class StagedApprenticeRequirement : IAuthorizationRequirement { }

public class StagedApprenticeAuthorizationHandler : AuthorizationHandler<StagedApprenticeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StagedApprenticeRequirement requirement)
    {
        HttpContext? currentContext;
        if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        switch (context.Resource)
        {
            case HttpContext resource:
                currentContext = resource;
                break;
            case AuthorizationFilterContext authorizationFilterContext:
                currentContext = authorizationFilterContext.HttpContext;
                break;
            default:
                currentContext = null;
                break;
        }

        if (currentContext != null && !context.User.IsStagedApprentice())
        {
            currentContext.Response.Redirect(@"/accessdenied");
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}