using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.Filters;

[ExcludeFromCodeCoverage]
public class RequiresRegistrationAuthorizationFilter : IAuthorizationFilter
{
    private readonly AuthenticatedUser _user;
    public RequiresRegistrationAuthorizationFilter(AuthenticatedUser user)
    {
        _user = user;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var url = WebUtility.UrlEncode(context.HttpContext.Request.GetUri().ToString());
        if (!_user.HasCreatedAccount)
            context.Result = new RedirectResult("https://localhost:7080?returnUrl=" + url);
    }
}
