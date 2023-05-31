using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticePortal.Authentication;

namespace SFA.DAS.ApprenticeAan.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class AuthenticationStartup
{
    public static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        AuthenticationConfiguration config,
        IWebHostEnvironment environment)
    {
        AddApplicationAuthentication(services, config, environment);
        AddApplicationAuthorisation(services);
        return services;
    }

    private static void AddApplicationAuthentication(IServiceCollection services, AuthenticationConfiguration config, IWebHostEnvironment environment)
    {
        var metadataAddress = config.MetadataAddress;
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = ".Apprenticeships.Application";
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = System.TimeSpan.FromHours(1);
                if (environment.EnvironmentName != "Development")
                    options.Cookie.Domain = ".apprenticeships.education.gov.uk";
            })
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = metadataAddress;
                options.RequireHttpsMetadata = false;
                options.ClientId = "apprentice";

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");

                options.SaveTokens = true;
                options.DisableTelemetry = false;

                options.EventsType = typeof(AuthenticationEventsLocal);
            });

        services.AddScoped<AuthenticationEventsLocal>();
        services.AddSingleton<IAuthorizationHandler, StagedApprenticeAuthorizationHandler>();
    }

    private static void AddApplicationAuthorisation(
        this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            var builder = new AuthorizationPolicyBuilder();
            builder.RequireAuthenticatedUser();
            builder.Requirements.Add(new StagedApprenticeRequirement());
            options.DefaultPolicy = builder.Build();
        });

        services.AddScoped<AuthenticatedUser>();
        services.AddScoped(s => s.GetRequiredService<IHttpContextAccessor>().HttpContext?.User ?? new());
    }
}
