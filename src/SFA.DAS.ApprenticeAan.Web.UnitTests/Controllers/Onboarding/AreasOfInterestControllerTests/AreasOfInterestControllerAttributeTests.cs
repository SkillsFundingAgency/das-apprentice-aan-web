using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.AreasOfInterestControllerTests;

[TestFixture]
public class AreasOfInterestControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectRouteAttribute()
    {
        typeof(AreasOfInterestController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(AreasOfInterestController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/areas-of-interest");
        typeof(AreasOfInterestController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be("AreasOfInterest");
    }
}