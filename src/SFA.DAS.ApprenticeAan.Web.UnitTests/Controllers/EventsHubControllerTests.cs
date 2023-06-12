using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class EventsHubControllerTests
{
    private IActionResult _result = null!;
    private static readonly string AllNetworksUrl = Guid.NewGuid().ToString();

    [SetUp]
    public async Task OnGet()
    {
        EventsHubController sut = new();
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.NetworkEvents, AllNetworksUrl);
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());

        _result = await sut.Index(null, null, new());
    }

    [Test]
    public void ThenReturnsView() => _result.Should().BeOfType<ViewResult>();

    [Test]
    public void ThenSetsViewModel() => _result.As<ViewResult>().Model.Should().BeOfType<EventsHubViewModel>();

    [Test]
    public void InvalidValue_ThenThrowsArgumentOutOfRangeException()
    {
        EventsHubController sut = new();
        Func<Task> action = () => sut.Index(13, null, new());
        action.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }
}
