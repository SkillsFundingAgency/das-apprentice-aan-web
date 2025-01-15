using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.RegionalNetworkControllerTests;

public class RegionalNetworkControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectAttributes()
    {
        typeof(RegionalNetworkController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(RegionalNetworkController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/regionalNetwork");
        typeof(RegionalNetworkController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.RegionalNetwork);
    }
}
