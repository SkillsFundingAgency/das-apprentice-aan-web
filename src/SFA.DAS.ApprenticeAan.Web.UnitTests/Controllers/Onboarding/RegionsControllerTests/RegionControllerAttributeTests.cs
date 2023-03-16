using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.RegionsControllerTests
{
    [TestFixture]
    public class RegionControllerAttributeTests
    {
        [Test]
        public void Controller_HasCorrectRouteAttribute()
        {
            typeof(RegionController).Should().BeDecoratedWith<RouteAttribute>();
            typeof(RegionController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/regions");
            typeof(RegionController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be("Regions");
        }

        [Test]
        public void Controller_HasRequiredSessionModelAttribute()
        {
            typeof(RegionController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
        }
    }
}
