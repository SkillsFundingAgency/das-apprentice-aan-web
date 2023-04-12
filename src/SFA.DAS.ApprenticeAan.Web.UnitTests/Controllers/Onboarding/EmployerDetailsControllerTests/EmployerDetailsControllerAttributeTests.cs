using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.EmployerDetailsControllerTests;

[TestFixture]
public class EmployerDetailsControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectAttributes()
    {

        typeof(EmployerDetailsController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(EmployerDetailsController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/employer-details");
        typeof(EmployerDetailsController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.EmployerDetails);
    }

    [Test]
    public void Controller_HasRequiredSessionModelAttribute()
    {
        typeof(EmployerDetailsController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
        typeof(EmployerDetailsController).Should().BeDecoratedWith<RequiredSessionModelAttribute>().Subject.ModelType.Name.Should().Be(nameof(OnboardingSessionModel));
    }
}