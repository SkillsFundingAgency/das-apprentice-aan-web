using System.Net;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.Filters;

public class RequiresRegistrationAuthorizationFilter : IAuthorizationFilter
{
    private readonly AuthenticatedUser _user;
    private readonly string _apprenticeAccountsUrl;
    public RequiresRegistrationAuthorizationFilter(AuthenticatedUser user, ApplicationConfiguration configuration)
    {
        _apprenticeAccountsUrl = configuration.ApplicationUrls.ApprenticeAccountsUrl.ToString().TrimEnd('/');
        _user = user;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (_user.HasCreatedAccount) return;

        var returnUrl = WebUtility.UrlEncode(context.HttpContext.Request.GetUri().ToString());
        var redirectUrl = string.Concat(_apprenticeAccountsUrl, "?returnUrl=", returnUrl);
        context.Result = new RedirectResult(redirectUrl);
    }
}
