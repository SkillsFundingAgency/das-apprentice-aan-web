using System.Diagnostics.CodeAnalysis;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.AppStart;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.ApprenticeAan.Web
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IConfiguration configuration)
        {
            var config = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .SetBasePath(Directory.GetCurrentDirectory());
#if DEBUG
            if (!configuration["EnvironmentName"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
                config.AddJsonFile("appsettings.json", true)
                    .AddJsonFile("appsettings.Development.json", true);
#endif
            config.AddEnvironmentVariables();

            if (!configuration["EnvironmentName"].Equals("DEV", StringComparison.CurrentCultureIgnoreCase))
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddConfigurationOptions();


            services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });


            services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; }).AddMvc(options =>
                {
                    if (!_configuration.IsDev()) options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                .AddSessionStateTempDataProvider();

            services.AddFluentValidationClientsideAdapters();

            services.AddHttpContextAccessor();

            if (_configuration.IsDev() || _configuration.IsLocal()) services.AddDistributedMemoryCache();

            services.AddHealthChecks();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
            });

            services.AddApplicationInsightsTelemetry();

            services.AddServiceRegistrations();

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

                app.UseStatusCodePagesWithReExecute("/error/{0}");
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.Use(async (context, next) =>
            {
                if (context.Response.Headers.ContainsKey("X-Frame-Options")) context.Response.Headers.Remove("X-Frame-Options");

                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

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
}