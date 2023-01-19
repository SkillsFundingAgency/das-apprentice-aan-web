using System.Diagnostics.CodeAnalysis;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Infrastructure.ApiClients;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.ApprenticeAan.Web.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddServiceRegistrationsExtension
    {
        public static void AddServiceRegistrations(this IServiceCollection services)
        {
            ConfigureHttpClient(services);
            services.AddHttpContextAccessor();
            services.AddTransient<ISessionService, SessionService>();
        }

        private static void ConfigureHttpClient(IServiceCollection services)
        {
            var handlerLifeTime = TimeSpan.FromMinutes(5);
            services.AddHttpClient<IApiClient, ApiClient>(config =>
                {
                    config.DefaultRequestHeaders.Add("Accept", "application/json");
                    config.DefaultRequestHeaders.Add("X-Version", "1");
                })
                .SetHandlerLifetime(handlerLifeTime);
        }
    }
}