using System.Diagnostics.CodeAnalysis;
using RestEase.HttpClientFactory;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Orchestrators;
using SFA.DAS.ApprenticeAan.Web.Services;
using SFA.DAS.ApprenticePortal.SharedUi.Services;
using SFA.DAS.Http.Configuration;

namespace SFA.DAS.ApprenticeAan.Web.AppStart;

[ExcludeFromCodeCoverage]
public static class ServiceRegistrationsStartup
{
    public static IServiceCollection AddServiceRegistrations(this IServiceCollection services, OuterApiConfiguration outerApiConfiguration)
    {
        AddOuterApi(services, outerApiConfiguration);
        services.AddTransient<IMenuVisibility, MenuVisibility>();
        services.AddTransient<ISessionService, SessionService>();
        services.AddTransient<IRegionService, RegionService>();
        services.AddTransient<IApprenticeAccountService, ApprenticeAccountService>();
        services.AddTransient<IApprenticeService, ApprenticeService>();
        services.AddTransient<IMemberService, MemberService>();
        services.AddTransient<IEventNotificationSettingsOrchestrator, EventNotificationSettingsOrchestrator>();

        return services;
    }

    private static void AddOuterApi(this IServiceCollection services, OuterApiConfiguration configuration)
    {
        services.AddTransient<IApimClientConfiguration>((_) => configuration);

        services.AddScoped<Http.MessageHandlers.DefaultHeadersHandler>();
        services.AddScoped<Http.MessageHandlers.LoggingMessageHandler>();
        services.AddScoped<Http.MessageHandlers.ApimHeadersHandler>();

        services
            .AddRestEaseClient<IOuterApiClient>(configuration.ApiBaseUrl)
            .AddHttpMessageHandler<Http.MessageHandlers.DefaultHeadersHandler>()
            .AddHttpMessageHandler<Http.MessageHandlers.ApimHeadersHandler>()
            .AddHttpMessageHandler<Http.MessageHandlers.LoggingMessageHandler>();
    }
}