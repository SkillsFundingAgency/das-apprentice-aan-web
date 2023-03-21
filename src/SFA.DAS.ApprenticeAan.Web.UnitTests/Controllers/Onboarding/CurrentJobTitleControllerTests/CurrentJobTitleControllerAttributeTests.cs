using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CurrentJobTitleControllerTests;

[TestFixture]
public class CurrentJobTitleControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectRouteAttribute()
    {
        typeof(CurrentJobTitleController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(CurrentJobTitleController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/current-job-title");
        typeof(CurrentJobTitleController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.CurrentJobTitle);
    }

    [Test]
    public void Controller_HasRequiredSessionModelAttribute()
    {
        typeof(CurrentJobTitleController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
    }
}