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
    private string currentTestMethodName;
    private NetworkHubViewModel model = null!;

    [SetUp]
    public void WhenGettingNetworkHub()
    {
        NetworkHubController sut = new();

        currentTestMethodName = TestContext.CurrentContext.Test.Name;

        if (currentTestMethodName == "ThenSetsEventsHubUrlInViewModel")
        {
            sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.EventsHub, EventsHubUrl);
        }
        else if (currentTestMethodName == "ThenSetsNetworkDirectoryUrlInViewModel")
        {
            sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.NetworkDirectory, NetworkDirectoryUrl);
        }
        else if (currentTestMethodName == "ThenReturnView")
        {
            sut.AddUrlHelperMock().AddUrlForRoute(SharedRouteNames.EventsHub, EventsHubUrl);
        }

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
    public void ThenSetsNetworkDirectoryUrlInViewModel()
    {
        model.NetworkDirectoryUrl.Should().Be(NetworkDirectoryUrl);
    }
}

