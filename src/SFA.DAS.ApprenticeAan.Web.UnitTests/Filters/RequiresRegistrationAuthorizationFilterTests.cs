using System.Net;
using System.Security.Claims;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Filters;

public class RequiresRegistrationAuthorizationFilterTests
{
    [Test, AutoData]
    public void OnAuthorization_UserHasAccount_DoesNotChangeContextResult(ApplicationConfiguration config)
    {
        AuthenticatedUser user = new(GetClaimsPrinciple(true));
        RequiresRegistrationAuthorizationFilter sut = new(user, config);

        var context = GetAuthContext();
        sut.OnAuthorization(context);

        context.Result.As<OkResult>().Should().NotBeNull();
    }

    [Test, AutoData]
    public void OnAuthorization_UserDoesNotHaveAccount_RedirectToApprenticeAccounts(ApplicationConfiguration config)
    {
        AuthenticatedUser user = new(GetClaimsPrinciple(false));
        RequiresRegistrationAuthorizationFilter sut = new(user, config);

        var context = GetAuthContext();
        sut.OnAuthorization(context);

        context.Result.As<RedirectResult>().Should().NotBeNull();
    }

    [TestCase("https://localhost:7080")]
    [TestCase("https://localhost:7080/")]
    public void OnAuthorization_UserDoesNotHaveAccount_AppendReturnUrlToRedirect(string accountsUrl)
    {
        ApplicationConfiguration config = new();
        config.ApplicationUrls.ApprenticeAccountsUrl = new Uri(accountsUrl);
        AuthenticatedUser user = new(GetClaimsPrinciple(false));
        RequiresRegistrationAuthorizationFilter sut = new(user, config);

        var context = GetAuthContext();
        var returnUrl = WebUtility.UrlEncode(context.HttpContext.Request.GetUri().ToString());
        var expectedRedirectUrl = string.Concat(accountsUrl.TrimEnd('/'), "?returnUrl=", returnUrl);

        sut.OnAuthorization(context);

        context.Result.As<RedirectResult>().Url.Should().Be(expectedRedirectUrl);
    }

    private static AuthorizationFilterContext GetAuthContext()
    {
        ActionContext actionContext = new(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        AuthorizationFilterContext context = new(actionContext, Enumerable.Empty<IFilterMetadata>().ToList());
        context.HttpContext.Request.Scheme = "https";
        context.Result = new OkResult();
        return context;
    }

    private static ClaimsPrincipal GetClaimsPrinciple(bool hasCreatedAccount)
    {
        ClaimsPrincipal claimsPrincipal = new();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(new[]
        {
            new Claim("AccountCreated", hasCreatedAccount ? "True" : "ItDoesNotMatter"),
            new Claim("apprentice_id", Guid.NewGuid().ToString()),
            new Claim("name", "abc@gmail.com")
        }));
        return claimsPrincipal;
    }
}
