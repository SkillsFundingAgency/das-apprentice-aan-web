using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.ReasonToJoinTheNetworkControllerTests;

[TestFixture]
public class ReasonToJoinTheNetworkControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectAttributes()
    {
        typeof(ReasonToJoinTheNetworkController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(ReasonToJoinTheNetworkController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/reason-to-join-the-network");
        typeof(ReasonToJoinTheNetworkController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.ReasonToJoinTheNetwork);
    }

    [Test]
    public void Controller_HasRequiredSessionModelAttribute()
    {
        typeof(ReasonToJoinTheNetworkController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
        typeof(ReasonToJoinTheNetworkController).Should().BeDecoratedWith<RequiredSessionModelAttribute>().Subject.ModelType.Name.Should().Be(nameof(OnboardingSessionModel));
    }
}