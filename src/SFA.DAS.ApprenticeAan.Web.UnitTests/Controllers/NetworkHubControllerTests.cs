using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class NetworkHubControllerTests
{
    private IActionResult _result = null!;
    static readonly string EventsHubUrl = Guid.NewGuid().ToString();
    static readonly string NetworkDirectoryUrl = Guid.NewGuid().ToString();
    static readonly string ProfileSettingsHubUrl = Guid.NewGuid().ToString();
    static readonly string ContactUsUrl = Guid.NewGuid().ToString();
    private NetworkHubViewModel model = null!;

    [SetUp]
    public void WhenGettingNetworkHub()
    {
        NetworkHubController sut = new();

        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.EventsHub, EventsHubUrl)
            .AddUrlForRoute(SharedRouteNames.NetworkDirectory, NetworkDirectoryUrl)
            .AddUrlForRoute(SharedRouteNames.ProfileSettings, ProfileSettingsHubUrl)
            .AddUrlForRoute(SharedRouteNames.ContactUs, ContactUsUrl);

        _result = sut.Index();
        model = _result.As<ViewResult>().Model.As<NetworkHubViewModel>();
    }

    [Test]
    public void ThenReturnView()
    {
        _result.As<ViewResult>().Should().NotBeNull();
    }

    [Test]
    public void ThenSetsEventsHubUrlInViewModel()
    {
        model.EventsHubUrl.Should().Be(EventsHubUrl);
    }

    [Test]
    public void ThenSetsProfileSettingsUrlInViewModel()
    {
        model.ProfileSettingsUrl.Should().Be(ProfileSettingsHubUrl);
    }

    [Test]
    public void ThenSetsNetworkDirectoryUrlInViewModel()
    {
        model.NetworkDirectoryUrl.Should().Be(NetworkDirectoryUrl);
    }

    [Test]
    public void ThenSetsContactUsUrlInViewModel()
    {
        model.ContactUsUrl.Should().Be(ContactUsUrl);
    }
}

