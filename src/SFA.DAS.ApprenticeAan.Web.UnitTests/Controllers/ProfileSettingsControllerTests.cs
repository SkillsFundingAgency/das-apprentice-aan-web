using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class ProfileSettingsControllerTests
{
    private IActionResult _result = null!;
    static readonly string YourNetworkProfileUrl = Guid.NewGuid().ToString();
    static readonly string LeaveTheNetworkUrl = Guid.NewGuid().ToString();

    [SetUp]
    public void WhenGettingNetworkHub()
    {
        ProfileSettingsController sut = new();
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourNetworkProfile, YourNetworkProfileUrl)
            .AddUrlForRoute(SharedRouteNames.LeaveTheNetwork, LeaveTheNetworkUrl);

        _result = sut.Index();
    }

    [Test]
    public void ThenReturnsView()
    {
        _result.As<ViewResult>().Should().NotBeNull();
    }

    [Test]
    public void ThenSetsYourNetworkProfileUrlInViewModel()
    {
        var model = _result.As<ViewResult>().Model.As<ProfileSettingsViewModel>();
        model.YourNetworkProfileUrl.Should().Be(YourNetworkProfileUrl);
    }

    [Test]
    public void ThenSetsLeaveTheNetworkUrlInViewModel()
    {
        var model = _result.As<ViewResult>().Model.As<ProfileSettingsViewModel>();
        model.LeaveTheNetworkUrl.Should().Be(LeaveTheNetworkUrl);
    }
}
