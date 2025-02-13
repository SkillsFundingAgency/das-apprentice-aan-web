using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.RegionalNetworkControllerTests;

public class RegionalNetworkControllerTests
{
    public void Get_ViewModel_SelectedRegionBasedOnSessionData()
    {
        var sessionServiceMock = new Mock<ISessionService>();
        RegionalNetworkController sut = new(sessionServiceMock.Object);
        sut.AddUrlHelperMock();
        OnboardingSessionModel sessionModel = new()
        {
            RegionName = "Test Region"
        };
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Model.As<RegionalNetworkViewModel>().SelectedRegion.Should().Be("Test Region");
    }

    public void Get_ViewModel_BackLinkBasedOnSessionData()
    {
        var sessionServiceMock = new Mock<ISessionService>();
        RegionalNetworkController sut = new(sessionServiceMock.Object);
        string expectedUrl = "test-url";

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.Regions, expectedUrl);
        OnboardingSessionModel sessionModel = new()
        {
            RegionName = "Test Region"
        };
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Model.As<RegionalNetworkViewModel>().BackLink.Should().Be(expectedUrl);
    }
}
