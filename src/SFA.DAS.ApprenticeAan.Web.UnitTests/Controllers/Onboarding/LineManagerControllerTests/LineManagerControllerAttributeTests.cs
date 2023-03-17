using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.LineManagerControllerTests;

[TestFixture]
public class LineManagerControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectAttributes()
    {
        typeof(LineManagerController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(LineManagerController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/line-manager");
        typeof(LineManagerController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be("LineManager");
    }

    [Test]
    public void Controller_HasRequiredSessionModelAttribute()
    {
        typeof(LineManagerController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
        typeof(LineManagerController).Should().BeDecoratedWith<RequiredSessionModelAttribute>().Subject.ModelType.Name.Should().Be(nameof(OnboardingSessionModel));
    }
}