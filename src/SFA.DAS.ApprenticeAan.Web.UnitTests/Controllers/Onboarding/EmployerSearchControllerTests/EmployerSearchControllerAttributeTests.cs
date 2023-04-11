using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.EmployerSearchControllerTests;

public class EmployerSearchControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectRouteAttribute()
    {
        typeof(EmployerSearchController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(EmployerSearchController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/employer-search");
        typeof(EmployerSearchController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.EmployerSearch);
    }

    [Test]
    public void Controller_HasRequiredSessionModelAttribute()
    {
        typeof(EmployerSearchController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
    }
}
