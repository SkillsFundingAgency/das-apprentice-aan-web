using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Web.Extensions;

[ExcludeFromCodeCoverage]
public static class ConfigurationExtensions
{
    public static bool IsDev(this IConfiguration configuration) => configuration["EnvironmentName"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase);
    public static bool IsLocal(this IConfiguration configuration) => configuration["EnvironmentName"].Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase);
}
