using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.DataProtection;
using SFA.DAS.ApprenticeAan.Web.AppStart;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using StackExchange.Redis;

namespace SFA.DAS.ApprenticeAan.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class DataProtectionStartup
{
    public static IServiceCollection AddDataProtection(
        this IServiceCollection services,
        ConnectionStringsConfiguration configuration,
        IWebHostEnvironment environment)
    {
        const string appName = "apprentice-portal";

        if (environment.IsDevelopment())
        {
            services.AddDistributedMemoryCache();
            services.AddDataProtection().SetApplicationName(appName);
        }
        else if (configuration != null)
        {
            var redisConnectionString = configuration.RedisConnectionString;
            var dataProtectionKeysDatabase = configuration.DataProtectionKeysDatabase;

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });

            var redis = ConnectionMultiplexer.Connect($"{redisConnectionString},{dataProtectionKeysDatabase}");

            services.AddDataProtection()
                .SetApplicationName(appName)
                .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
        }

        return services;
    }
}
