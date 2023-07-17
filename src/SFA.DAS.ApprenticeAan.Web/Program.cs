using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.ApprenticeAan.Web.AppStart;
using SFA.DAS.ApprenticeAan.Web.Authentication;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.HealthCheck;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;
using SFA.DAS.ApprenticePortal.SharedUi.Startup;

var builder = WebApplication.CreateBuilder(args);

var rootConfiguration = builder.Configuration.LoadConfiguration(builder.Services);

var environmentName = rootConfiguration["EnvironmentName"];
var applicationConfiguration = rootConfiguration.Get<ApplicationConfiguration>();
builder.Services.AddSingleton(applicationConfiguration);

builder.Services
    .AddOptions()
    .AddLogging()
    .AddApplicationInsightsTelemetry()
    .AddHttpContextAccessor()
    .AddValidatorsFromAssembly(typeof(RegionsSubmitModelValidator).Assembly)
    .AddSession(environmentName, applicationConfiguration.ConnectionStrings)
    .AddDataProtection(applicationConfiguration.ConnectionStrings, builder.Environment)
    .AddAuthentication(applicationConfiguration.Authentication, builder.Environment)
    .AddServiceRegistrations(applicationConfiguration.ApprenticeAanOuterApi);

builder.Services.AddHealthChecks()
    .AddCheck<ApprenticeAanOuterApiHealthCheck>(ApprenticeAanOuterApiHealthCheck.HealthCheckResultDescription,
    failureStatus: HealthStatus.Unhealthy,
    tags: new[] { "ready" });

builder.Services.AddSharedUi(applicationConfiguration, options =>
{
    /// We dont have a menu yet so cannot set this
    /// options.SetCurrentNavigationSection(NavigationSection.ApprenticeFeedback);
    options.EnableZendesk();
    options.EnableGoogleAnalytics();
});

builder.Services
    .Configure<RouteOptions>(options => { options.LowercaseUrls = true; })
    .AddMvc(options =>
    {
        options.Filters.Add<RequiresRegistrationAuthorizationFilter>();
        options.Filters.Add<RequiresExistingMemberAttribute>();
        options.Filters.Add<RequiresSessionModelAttribute>();
        options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
    })
    .AddSessionStateTempDataProvider();

#if DEBUG
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
#endif


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHealthChecks("/ping");
    /// app.UseStatusCodePagesWithReExecute("/error/{0}"); 
    /// app.UseExceptionHandler("/error");
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseCookiePolicy()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseSession()
    .UseHealthChecks()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            "default",
            "{controller=Home}/{action=Index}/{id?}");
    });

app.Run();

