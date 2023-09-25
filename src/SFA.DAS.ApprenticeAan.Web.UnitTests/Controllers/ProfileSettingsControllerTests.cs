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
    static readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();
    static readonly string LeaveTheNetworkUrl = Guid.NewGuid().ToString();

    [SetUp]
    public void WhenGettingNetworkHub()
    {
        ProfileSettingsController sut = new();
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl)
            .AddUrlForRoute(SharedRouteNames.LeaveTheNetwork, LeaveTheNetworkUrl);

        _result = sut.Index();
    }

    [Test]
    public void ThenReturnsView()
    {
        _result.As<ViewResult>().Should().NotBeNull();
    }

    [Test]
    public void ThenSetsYourAmbassadorProfileUrlInViewModel()
    {
        var model = _result.As<ViewResult>().Model.As<ProfileSettingsViewModel>();
        model.YourAmbassadorProfileUrl.Should().Be(YourAmbassadorProfileUrl);
    }

    [Test]
    public void ThenSetsLeaveTheNetworkUrlInViewModel()
    {
        var model = _result.As<ViewResult>().Model.As<ProfileSettingsViewModel>();
        model.LeaveTheNetworkUrl.Should().Be(LeaveTheNetworkUrl);
    }
}
