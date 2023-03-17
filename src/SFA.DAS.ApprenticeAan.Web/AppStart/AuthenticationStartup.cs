using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
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

        AddApplicationAuthorisation(services);

        return services;
    }


    public class AuthenticationEventsLocal : OpenIdConnectEvents
    {
        private readonly IOuterApiClient _client;

        public AuthenticationEventsLocal(IOuterApiClient client)
        {
            _client = client;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            await base.TokenValidated(context);
            ConvertRegistrationIdToApprenticeId(context.Principal!);
            await AddClaims(context.Principal!);
        }

        public void ConvertRegistrationIdToApprenticeId(ClaimsPrincipal principal)
        {
            var registrationClaim = principal.Claims.FirstOrDefault(c => c.Type == "registration_id");
            var apprenticeClaim = principal.ApprenticeIdClaim();

            if (registrationClaim == null) return;
            if (apprenticeClaim != null) return;

            principal.AddApprenticeIdClaim(registrationClaim.Value);
        }

        public async Task AddClaims(ClaimsPrincipal principal)
        {
            var apprentice = await GetApprentice(principal);
            if (apprentice == null)
                return;

            AddApprenticeAccountClaims(principal, apprentice);
        }

        private async Task<IApprenticeAccount?> GetApprentice(ClaimsPrincipal principal)
        {
            var claim = principal.ApprenticeIdClaim();

            if (!Guid.TryParse(claim?.Value, out var apprenticeId)) return null;

            try
            {
                var app = await _client.GetApprenticeAccount(apprenticeId);
                return app;
            }
            catch (RestEase.ApiException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
                throw;
            }
        }

        public static async Task UserAccountCreated(HttpContext context, IApprenticeAccount apprentice)
        {
            var authenticated = await context.AuthenticateAsync();

            if (authenticated.Succeeded)
            {
                AddApprenticeAccountClaims(authenticated.Principal, apprentice);
                await context.SignInAsync(authenticated.Principal, authenticated.Properties);
            }
        }

        public static async Task UserAccountUpdated(HttpContext context, IApprenticeAccount apprentice)
        {
            var authenticated = await context.AuthenticateAsync();

            if (authenticated.Succeeded)
            {
                UpdateApprenticeAccountClaims(authenticated.Principal, apprentice);
                await context.SignInAsync(authenticated.Principal, authenticated.Properties);
            }
        }

        public static async Task TermsOfUseAccepted(HttpContext context)
        {
            var authenticated = await context.AuthenticateAsync();

            if (authenticated.Succeeded)
            {
                authenticated.Principal.AddTermsOfUseAcceptedClaim();
                await context.SignInAsync(authenticated.Principal, authenticated.Properties);
            }
        }

        private static void AddNameClaims(ClaimsPrincipal principal, IApprenticeAccount apprentice)
        {
            principal.AddIdentity(new ClaimsIdentity(new[]
            {
            new Claim(IdentityClaims.GivenName, apprentice.FirstName),
            new Claim(IdentityClaims.FamilyName, apprentice.LastName),
        }));
        }

        private static void RemoveNameClaims(ClaimsIdentity identity)
        {
            RemoveClaim(identity, IdentityClaims.GivenName);
            RemoveClaim(identity, IdentityClaims.FamilyName);
        }

        private static void RemoveClaim(ClaimsIdentity identity, string ClaimType)
        {
            var claim = identity.FindFirst(ClaimType);
            if (claim != null)
                identity.RemoveClaim(claim);
        }

        private static void AddApprenticeAccountClaims(ClaimsPrincipal principal, IApprenticeAccount apprentice)
        {
            principal.AddAccountCreatedClaim();

            AddNameClaims(principal, apprentice);

            if (apprentice.TermsOfUseAccepted)
                principal.AddTermsOfUseAcceptedClaim();
        }

        private static void UpdateApprenticeAccountClaims(ClaimsPrincipal principal, IApprenticeAccount apprentice)
        {
            if (principal.Identity is ClaimsIdentity identity)
            {
                RemoveNameClaims(identity);
            }

            AddNameClaims(principal, apprentice);
        }
    }


    //private static IServiceCollection AddAuthenticationOld(
    //    this IServiceCollection services,
    //    AuthenticationConfiguration config,
    //    IWebHostEnvironment environment)
    //{
    //    services
    //        .AddApplicationAuthentication(config, environment)
    //        .AddApplicationAuthorisation();

    //    services.AddTransient((_) => config);

    //    return services;
    //}

    //private static IServiceCollection AddApplicationAuthentication(
    //    this IServiceCollection services,
    //    AuthenticationConfiguration config,
    //    IWebHostEnvironment environment)
    //{
    //    JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    //    services.AddApprenticeAuthentication(config.MetadataAddress, environment);

    //    services.AddTransient<IApprenticeAccountProvider, ApprenticeAccountProvider>();

    //    return services;
    //}

    private static void AddApplicationAuthorisation(
        this IServiceCollection services)
    {
        services.AddAuthorization();

        services.AddScoped<AuthenticatedUser>();
        services.AddScoped(s => s
            .GetRequiredService<IHttpContextAccessor>().HttpContext?.User ?? new());
    }
}
