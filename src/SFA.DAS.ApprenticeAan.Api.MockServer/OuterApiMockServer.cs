using WireMock.Logging;
using WireMock.Net.StandAlone;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Settings;

namespace SFA.DAS.ApprenticeAan.Api.MockServer;

internal static class OuterApiMockServer
{
    public static void Run()
    {
        var settings = new WireMockServerSettings
        {
            Port = 7054,
            UseSSL = true,
            Logger = new WireMockConsoleLogger()
        };

        var server = StandAloneApp.Start(settings);

        server
            .Given(Request.Create().WithPath(u => u.Contains("regions"))
            .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("Data/regions.json"));

        server
            .Given(Request.Create().WithPath(u => u.Contains("profiles"))
            .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("Data/profiles.json"));

        server
            .Given(Request.Create().WithPath(u => u.Contains("account"))
            .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("Data/apprentice-account.json"));

        server
            .Given(Request.Create().WithPath(u => u.Contains("locations"))
            .UsingGet())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/json")
                .WithBodyFromFile("Data/locations.json"));
    }
}
