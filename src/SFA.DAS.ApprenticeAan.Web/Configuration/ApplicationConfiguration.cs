﻿using System.Diagnostics.CodeAnalysis;
using SFA.DAS.ApprenticePortal.SharedUi;
using SFA.DAS.ApprenticePortal.SharedUi.GoogleAnalytics;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;
using SFA.DAS.ApprenticePortal.SharedUi.Zendesk;
using SFA.DAS.Http.Configuration;

namespace SFA.DAS.ApprenticeAan.Web.Configuration;

[ExcludeFromCodeCoverage]
public class ApplicationConfiguration : ISharedUiConfiguration
{
    public NavigationSectionUrls ApplicationUrls { get; set; } = null!;

    public GoogleAnalyticsConfiguration GoogleAnalytics { get; set; } = null!;

    public ZenDeskConfiguration Zendesk { get; set; } = null!;

    public ConnectionStringsConfiguration ConnectionStrings { get; set; } = null!;

    public AuthenticationConfiguration Authentication { get; set; } = null!;

    public OuterApiConfiguration ApprenticeAanOuterApi { get; set; } = null!;
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