using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseContentSecurityPolicy(this IApplicationBuilder app)
    {
        const string dasCdn = "das-at-frnt-end.azureedge.net das-pp-frnt-end.azureedge.net das-mo-frnt-end.azureedge.net das-test-frnt-end.azureedge.net das-test2-frnt-end.azureedge.net das-prd-frnt-end.azureedge.net";
        app.Use(async (context, next) =>
        {
            context.Response.Headers["Content-Security-Policy"] =
                $"script-src 'self' 'unsafe-inline' {dasCdn} *.tagmanager.google.com https://ssl.google-analytics.com *.googletagmanager.com *.google-analytics.com *.googleapis.com https://*.services.visualstudio.com https://*.rcrsv.io; " +
                $"style-src 'self' 'unsafe-inline' {dasCdn} *.tagmanager.google.com https://fonts.googleapis.com https://*.rcrsv.io; " +
                $"img-src {dasCdn} *.googletagmanager.com https://ssl.gstatic.com https://www.gstatic.com *.google-analytics.com https://*.rcrsv.io; " +
                $"font-src {dasCdn} https://fonts.gstatic.com https://*.rcrsv.io; " +
                "connect-src 'self' *.google-analytics.com https://*.rcrsv.io; " +
                "frame-src *.googletagmanager.com https://*.rcrsv.io";

            await next();
        });

        return app;
    }
}