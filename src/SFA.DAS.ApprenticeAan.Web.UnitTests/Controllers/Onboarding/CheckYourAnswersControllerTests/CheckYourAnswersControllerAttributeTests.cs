using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using System.Security.Policy;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.CheckYourAnswersControllerTests;

[TestFixture]
public class CheckYourAnswersControllerAttributeTests
{
    [Test]
    public void Controller_HasCorrectRouteAttribute()
    {
        typeof(CheckYourAnswersController).Should().BeDecoratedWith<RouteAttribute>();
        typeof(CheckYourAnswersController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/check-your-answers");
        typeof(CheckYourAnswersController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }

    [Test]
    public void Controller_HasRequiredSessionModelAttribute()
    {
        typeof(CheckYourAnswersController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
    }
}