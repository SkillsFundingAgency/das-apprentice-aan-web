using System.Diagnostics.CodeAnalysis;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.AppStart;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;
using SFA.DAS.ApprenticePortal.SharedUi.Startup;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.ApprenticeAan.Web;

[ExcludeFromCodeCoverage]
public class Startup
{
    private readonly IConfigurationRoot _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _environment = environment;

        var config = new ConfigurationBuilder()
            .AddConfiguration(configuration)
            .SetBasePath(Directory.GetCurrentDirectory());

        if (!configuration.IsDev())
            config.AddAzureTableStorage(options =>
                {
                    options.ConfigurationKeys = configuration["ConfigNames"].Split(",");
                    options.StorageConnectionString = configuration["ConfigurationStorageConnectionString"];
                    options.EnvironmentName = configuration["EnvironmentName"];
                    options.PreFixConfigurationKeys = false;
                }
            );

        _configuration = config.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var appConfig = _configuration.Get<ApplicationConfiguration>();

        services
            .AddApplicationInsightsTelemetry()
            .AddDataProtection(appConfig.ConnectionStrings, _environment)
            //TODO
            //.AddAuthentication(appConfig.Authentication, _environment)
            //.AddOuterApi(appConfig.ApprenticeFeedbackOuterApi)
            .AddHttpContextAccessor()
            .AddServiceRegistrations();

        services.AddSharedUi(appConfig, options =>
        {
            options.SetCurrentNavigationSection(NavigationSection.ApprenticeFeedback); //TODO to change the navigation section
            options.EnableZendesk();
            options.EnableGoogleAnalytics();
        });

        services.AddSession(options =>
        {
            options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
        });


        services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; }).AddMvc(options =>
        {
            if (!_configuration.IsDev()) options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        })
        .AddSessionStateTempDataProvider();

        services
            .AddFluentValidationClientsideAdapters()
            .AddFluentValidationAutoValidation();

        services.AddHttpContextAccessor();

        if (_configuration.IsDev() || _configuration.IsLocal()) services.AddDistributedMemoryCache();

        services.AddHealthChecks();

#if DEBUG
        services.AddControllersWithViews().AddRazorRuntimeCompilation();
#endif
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) { app.UseDeveloperExceptionPage(); }
        else
        {
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
                });

            app.UseStatusCodePagesWithReExecute("/error/{0}"); //TODO add error controller and pages
            app.UseExceptionHandler("/error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        app.UseRouting();
        app.UseMiddleware<SecurityHeadersMiddleware>();

        //TODO Renable after authentication
        //app.UseAuthentication();
        //app.UseAuthorization();
        app.UseSession();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
        });
    }
}