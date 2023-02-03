using System.Diagnostics.CodeAnalysis;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.ApprenticeAan.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class ServiceRegistrationsStartup
{
    public static void AddServiceRegistrations(this IServiceCollection services)
    {
        ConfigureHttpClient(services);
        services.AddTransient<ISessionService, SessionService>();
    }

    private static void ConfigureHttpClient(IServiceCollection services)
    {
        //TODO
        //var handlerLifeTime = TimeSpan.FromMinutes(5);
        //services.AddHttpClient<IApiClient, ApiClient>(config =>
        //    {
        //        config.DefaultRequestHeaders.Add("Accept", "application/json");
        //        config.DefaultRequestHeaders.Add("X-Version", "1");
        //    })
        //    .SetHandlerLifetime(handlerLifeTime);
    }
}