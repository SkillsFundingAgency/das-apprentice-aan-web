using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.ApprenticeAan.Web.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class ConfigurationExtensions
    {
        public static void LoadConfiguration(this ConfigurationManager configuration)
        {
            configuration.AddAzureTableStorage(options =>
            {
                options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                options.EnvironmentName = configuration["EnvironmentName"];
                options.PreFixConfigurationKeys = false;
            });
        }
    }
}