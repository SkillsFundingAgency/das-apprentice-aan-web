using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.Filters;

[ExcludeFromCodeCoverage]
public class RequiresRegistrationAuthorizationFilter : IAuthorizationFilter
{
    private readonly AuthenticatedUser _user;
    private readonly string _apprenticeLoginUrl;
    public RequiresRegistrationAuthorizationFilter(AuthenticatedUser user, ApplicationConfiguration configuration)
    {
        _apprenticeLoginUrl = configuration.ApplicationUrls.ApprenticeAccountsUrl.ToString().TrimEnd('/');
        _user = user;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var returnUrl = WebUtility.UrlEncode(context.HttpContext.Request.GetUri().ToString());
        var redirectUrl = string.Concat(_apprenticeLoginUrl, "?returnUrl=", returnUrl);
        if (!_user.HasCreatedAccount)
            context.Result = new RedirectResult(redirectUrl);
    }
}
