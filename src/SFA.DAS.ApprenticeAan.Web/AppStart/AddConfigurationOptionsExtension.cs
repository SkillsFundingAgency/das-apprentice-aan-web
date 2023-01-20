using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Web.AppStart
{
    [ExcludeFromCodeCoverage]
    public static class AddConfigurationOptionsExtension
    {
        public static void AddConfigurationOptions(this IServiceCollection services) => services.AddOptions();
    }
}