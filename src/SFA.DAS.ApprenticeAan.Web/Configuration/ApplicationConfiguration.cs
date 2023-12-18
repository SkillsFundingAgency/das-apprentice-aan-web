using SFA.DAS.ApprenticePortal.SharedUi;
using SFA.DAS.ApprenticePortal.SharedUi.GoogleAnalytics;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;
using SFA.DAS.ApprenticePortal.SharedUi.Zendesk;
using SFA.DAS.Http.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeAan.Web.Configuration;

[ExcludeFromCodeCoverage]
public class ApplicationConfiguration : ISharedUiConfiguration
{
    public NavigationSectionUrls ApplicationUrls { get; set; } = new();
    public ContactUsEmails ContactUsEmails { get; set; } = new();

    public GoogleAnalyticsConfiguration GoogleAnalytics { get; set; } = new();

    public ZenDeskConfiguration Zendesk { get; set; } = new();

    public ConnectionStringsConfiguration ConnectionStrings { get; set; } = new();

    public AuthenticationConfiguration Authentication { get; set; } = new();

    public OuterApiConfiguration ApprenticeAanOuterApi { get; set; } = new();
}

public class ContactUsEmails
{
    public string EastOfEngland { get; set; } = null!;
    public string EastMidlands { get; set; } = null!;
    public string London { get; set; } = null!;
    public string NorthEast { get; set; } = null!;
    public string NorthWest { get; set; } = null!;
    public string SouthEast { get; set; } = null!;
    public string SouthWest { get; set; } = null!;
    public string WestMidlands { get; set; } = null!;
    public string YorkshireAndTheHumber { get; set; } = null!;
}

public class ConnectionStringsConfiguration
{
    public string RedisConnectionString { get; set; } = null!;
    public string DataProtectionKeysDatabase { get; set; } = null!;
}

public class AuthenticationConfiguration
{
    public string MetadataAddress { get; set; } = null!;
}

public class OuterApiConfiguration : IApimClientConfiguration
{
    public string ApiBaseUrl { get; set; } = null!;

    public string SubscriptionKey { get; set; } = null!;

    public string ApiVersion { get; set; } = null!;
}
