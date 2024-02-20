using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.ApprenticeAan.Web.AppStart;
using SFA.DAS.ApprenticeAan.Web.Authentication;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.HealthCheck;
using SFA.DAS.ApprenticeAan.Web.Validators.MemberProfile;
using SFA.DAS.ApprenticeAan.Web.Validators.Onboarding;
using SFA.DAS.ApprenticePortal.SharedUi.Startup;
using SFA.DAS.Telemetry.Startup;

var builder = WebApplication.CreateBuilder(args);

var rootConfiguration = builder.Configuration.LoadConfiguration(builder.Services);

var environmentName = rootConfiguration["EnvironmentName"];
var applicationConfiguration = rootConfiguration.Get<ApplicationConfiguration>();
IEnumerable<string> tag = new[] { "ready" };

builder.Services.AddSingleton(applicationConfiguration!);

builder.Services
    .AddOptions()
    .AddLogging()
    .AddApplicationInsightsTelemetry()
    .AddTelemetryUriRedaction("firstName,lastName,dateOfBirth,email")
    .AddHttpContextAccessor()
    .AddValidatorsFromAssembly(typeof(RegionsSubmitModelValidator).Assembly)
    .AddValidatorsFromAssembly(typeof(ConnectWithMemberSubmitModelValidator).Assembly)
    .AddSession(environmentName!, applicationConfiguration!.ConnectionStrings)
    .AddDataProtection(applicationConfiguration.ConnectionStrings, builder.Environment)
    .AddAuthentication(applicationConfiguration.Authentication, builder.Environment)
    .AddServiceRegistrations(applicationConfiguration.ApprenticeAanOuterApi);

builder.Services.AddHealthChecks()
    .AddCheck<ApprenticeAanOuterApiHealthCheck>(ApprenticeAanOuterApiHealthCheck.HealthCheckResultDescription,
    failureStatus: HealthStatus.Unhealthy,
    tags: tag);

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
    app.UseHsts();
}

app.UseExceptionHandler("/error");

app.Use(async (context, next) =>
{
    if (context.Response.Headers.ContainsKey("X-Frame-Options"))
    {
        context.Response.Headers.Remove("X-Frame-Options");
    }

    context.Response.Headers!.Append("X-Frame-Options", "SAMEORIGIN");

    await next();

    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        //Re-execute the request so the user gets the error page
        var originalPath = context.Request.Path.Value;
        context.Items["originalPath"] = originalPath;
        context.Request.Path = "/error/404";
        await next();
    }
});

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

