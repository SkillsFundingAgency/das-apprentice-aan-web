using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.ReasonToJoinControllerTests;

[TestFixture]
public class ReasonToJoinControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectAttributes()
    {
        typeof(ReasonToJoinController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(ReasonToJoinController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/reason-to-join");
        typeof(ReasonToJoinController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.ReasonToJoin);
    }
}