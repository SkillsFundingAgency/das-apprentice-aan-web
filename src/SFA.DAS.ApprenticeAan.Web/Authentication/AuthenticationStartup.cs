using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApprenticeAan.Web.Authentication;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Services;
using SFA.DAS.ApprenticePortal.Authentication;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Authentication;

[ExcludeFromCodeCoverage]
public static class AuthenticationStartup
{
    public static void AddGovLoginAuthentication(
        this IServiceCollection services,
        NavigationSectionUrls config,
        IConfiguration configuration)
    {
        services.AddGovLoginAuthentication(configuration);
        services.AddAuthorization(options =>
        {
            var builder = new AuthorizationPolicyBuilder();
            builder.RequireAuthenticatedUser();
            options.DefaultPolicy = builder.Build();
        });

        services.AddScoped<AuthenticatedUser>();
        services.AddHttpContextAccessor();
        services.AddTransient((_) => config);
    }
}
