using SFA.DAS.ApprenticeAan.Web.Configuration;

namespace SFA.DAS.ApprenticeAan.Web.AppStart;

public static class AddSessionExtension
{
    public static IServiceCollection AddSession(this IServiceCollection services, string environmentName, ConnectionStringsConfiguration connectionStrings)
    {
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(10);
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.IsEssential = true;
        });

        if (environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionStrings.RedisConnectionString;
            });
        }

        return services;
    }
}
