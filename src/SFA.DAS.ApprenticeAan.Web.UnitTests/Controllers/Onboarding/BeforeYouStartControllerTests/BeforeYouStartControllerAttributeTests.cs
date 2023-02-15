using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.BeforeYouStartControllerTests;

[TestFixture]
public class BeforeYouStartControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectAttributes()
    {
        typeof(BeforeYouStartController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(BeforeYouStartController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/before-you-start");
        typeof(BeforeYouStartController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.BeforeYouStart);
    }
}
