using System.Diagnostics.CodeAnalysis;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.ApprenticeAan.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class LoadConfigurationExtension
{
    public static IConfigurationRoot LoadConfiguration(this IConfiguration config, IServiceCollection services)
    {
        var configBuilder = new ConfigurationBuilder()
            .AddConfiguration(config)
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables();


        if (!config["EnvironmentName"]!.Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
        {
            configBuilder.AddAzureTableStorage(options =>
            {
                options.ConfigurationKeys = config["ConfigNames"]!.Split(",");
                options.StorageConnectionString = config["ConfigurationStorageConnectionString"];
                options.EnvironmentName = config["EnvironmentName"];
                options.PreFixConfigurationKeys = false;
            });
        }

        var configuration = configBuilder.Build();

        var appConfig = configuration.Get<ApplicationConfiguration>();
        services.AddSingleton(appConfig!);

        return configuration;
    }
}