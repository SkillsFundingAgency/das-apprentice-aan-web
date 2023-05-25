using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.AppStart;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticePortal.SharedUi.Startup;
using SFA.DAS.Configuration.AzureTableStorage;
using System.Diagnostics.CodeAnalysis;

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
        services.AddSingleton(appConfig);

        services
            .AddLogging()
            .AddApplicationInsightsTelemetry()
            .AddDataProtection(appConfig.ConnectionStrings, _environment)
            .AddAuthentication(appConfig.Authentication, _environment)
            .AddHttpContextAccessor()
            .AddServiceRegistrations(appConfig.ApprenticeAanOuterApi)
            .AddHealthChecks();

        services.AddSharedUi(appConfig, options =>
        {
            /// We dont have a menu yet so cannot set this
            /// options.SetCurrentNavigationSection(NavigationSection.ApprenticeFeedback);
            options.EnableZendesk();
            options.EnableGoogleAnalytics();
        });

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.IsEssential = true;
        });

        services
            .Configure<RouteOptions>(options => { options.LowercaseUrls = true; })
            .AddMvc(options =>
            {
                options.Filters.Add(new RequiresExistingMemberAttribute());
                options.Filters.Add(new RequiresSessionModelAttribute());
                options.Filters.Add<RequiresRegistrationAuthorizationFilter>();
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            .AddSessionStateTempDataProvider();

        services.AddValidatorsFromAssembly(typeof(LineManagerViewModel).Assembly);

        if (_configuration.IsDev() || _configuration.IsLocal()) services.AddDistributedMemoryCache();

#if DEBUG
        services.AddControllersWithViews().AddRazorRuntimeCompilation();
#endif
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHealthChecks("/health"); //TODO add outer api health check
            app.UseStatusCodePagesWithReExecute("/error/{0}"); //TODO add error controller and pages
            app.UseExceptionHandler("/error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
        });
    }
}