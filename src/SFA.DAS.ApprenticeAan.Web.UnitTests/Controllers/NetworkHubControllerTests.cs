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

    [SetUp]
    public void WhenGettingNetworkHub()
    {
        NetworkHubController sut = new();
        sut.AddUrlHelperMock();
        _result = sut.Index();
    }

    [Test]
    public void ThenReturnsView()
    {
        NetworkHubController sut = new();
        sut.AddUrlHelperMock();
        _result = sut.Index();
        _result.As<ViewResult>().Should().NotBeNull();
    }

    [Test]
    public void SetsEventsHubUrlInViewModel()
    {
        NetworkHubController sut = new();
        sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.EventsHub, EventsHubUrl);
        _result = sut.Index();

        var model = _result.As<ViewResult>().Model.As<NetworkHubViewModel>();
        model.EventsHubUrl.Should().Be(EventsHubUrl);
    }

    [Test]
    public void SetsNetworkDirectoryUrlInViewModel()
    {
        NetworkHubController sut = new();
        sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.NetworkDirectory, NetworkDirectoryUrl);
        _result = sut.Index();

        var model = _result.As<ViewResult>().Model.As<NetworkHubViewModel>();
        model.NetworkDirectoryUrl.Should().Be(NetworkDirectoryUrl);
    }
}
