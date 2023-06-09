using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class NetworkHubControllerTests
{
    private IActionResult _result = null!;
    static readonly string EventsHubUrl = Guid.NewGuid().ToString();

    [SetUp]
    public void WhenGettingNetworkHub()
    {
        NetworkHubController sut = new();
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.EventsHub, EventsHubUrl);

        _result = sut.Index();
    }

    [Test]
    public void ThenReturnsView()
    {
        _result.As<ViewResult>().Should().NotBeNull();
    }

    [Test]
    public void ThenSetsEventsHubUrlInViewModel()
    {
        var model = _result.As<ViewResult>().Model.As<NetworkHubViewModel>();
        model.EventsHubUrl.Should().Be(EventsHubUrl);
    }
}
