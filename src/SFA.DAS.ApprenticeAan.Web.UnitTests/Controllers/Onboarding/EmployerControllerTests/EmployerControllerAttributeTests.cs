using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using System.Reflection;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.EmployerControllerTests;

[TestFixture]
public class EmployerControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectAttributes()
    {

        typeof(EmployerController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(EmployerController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/employer-details");
        typeof(EmployerController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.EmployerDetails);
    }

    [Test]
    public void Controller_HasRequiredSessionModelAttribute()
    {
        typeof(EmployerController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
        typeof(EmployerController).Should().BeDecoratedWith<RequiredSessionModelAttribute>().Subject.ModelType.Name.Should().Be(nameof(OnboardingSessionModel));
    }
}