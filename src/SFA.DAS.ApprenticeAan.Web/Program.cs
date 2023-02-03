using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore;
using NLog.Web;

namespace SFA.DAS.ApprenticeAan.Web;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static void Main(string[] args)
    {
        NLogBuilder.ConfigureNLog("nlog.config");
        CreateHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseNLog();
}