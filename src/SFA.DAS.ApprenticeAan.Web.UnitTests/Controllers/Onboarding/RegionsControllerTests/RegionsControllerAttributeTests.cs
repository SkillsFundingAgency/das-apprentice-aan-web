using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.RegionsControllerTests;

[TestFixture]
public class RegionsControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectRouteAttribute()
    {
        typeof(RegionsController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(RegionsController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/regions");
        typeof(RegionsController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be("Regions");
    }

    [Test]
    public void Controller_HasRequiredSessionModelAttribute()
    {
        typeof(RegionsController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
    }
}