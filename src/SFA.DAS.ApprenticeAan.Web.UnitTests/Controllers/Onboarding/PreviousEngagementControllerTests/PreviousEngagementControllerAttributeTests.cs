using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.PreviousEngagementControllerTests;

[TestFixture]
public class PreviousEngagementControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectAttributes()
    {
        typeof(PreviousEngagementController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(PreviousEngagementController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/previous-engagement");
        typeof(PreviousEngagementController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.PreviousEngagement);
    }

    [Test]
    public void Controller_HasRequiredSessionModelAttribute()
    {
        typeof(PreviousEngagementController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
        typeof(PreviousEngagementController).Should().BeDecoratedWith<RequiredSessionModelAttribute>().Subject.ModelType.Name.Should().Be(nameof(OnboardingSessionModel));
    }
}