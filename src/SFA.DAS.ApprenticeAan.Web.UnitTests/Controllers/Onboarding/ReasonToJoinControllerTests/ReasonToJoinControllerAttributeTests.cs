using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.ReasonToJoinControllerTests;

[TestFixture]
public class ReasonToJoinControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectAttributes()
    {
        typeof(ReasonToJoinController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(ReasonToJoinController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/join-the-network");
        typeof(ReasonToJoinController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.ReasonToJoin);
    }

    [Test]
    public void Controller_HasRequiredSessionModelAttribute()
    {
        typeof(ReasonToJoinController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
        typeof(ReasonToJoinController).Should().BeDecoratedWith<RequiredSessionModelAttribute>().Subject.ModelType.Name.Should().Be(nameof(OnboardingSessionModel));
    }
}