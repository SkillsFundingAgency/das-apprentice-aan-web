using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class EventsHubControllerTests
{
    private IActionResult _result = null!;
    private static readonly string AllNetworksUrl = Guid.NewGuid().ToString();

    [SetUp]
    public void OnGet()
    {
        EventsHubController sut = new();
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.NetworkEvents, AllNetworksUrl);

        _result = sut.Index();
    }

    [Test]
    public void ThenReturnsView()
    {

        _result.Should().BeOfType<ViewResult>();
    }

    [Test]
    public void ThenSetsNetworkEventUrl()
    {
        _result.As<ViewResult>().Model.As<EventsHubViewModel>().AllNetworksUrl.Should().Be(AllNetworksUrl);
    }
}
