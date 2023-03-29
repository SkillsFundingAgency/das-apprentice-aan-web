using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.JoinTheNetworkControllerTests;

[TestFixture]
public class JoinTheNetworkControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectAttributes()
    {
        typeof(JoinTheNetworkController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(JoinTheNetworkController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/join-the-network");
        typeof(JoinTheNetworkController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.JoinTheNetwork);
    }

    [Test]
    public void Controller_HasRequiredSessionModelAttribute()
    {
        typeof(JoinTheNetworkController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
        typeof(JoinTheNetworkController).Should().BeDecoratedWith<RequiredSessionModelAttribute>().Subject.ModelType.Name.Should().Be(nameof(OnboardingSessionModel));
    }
}