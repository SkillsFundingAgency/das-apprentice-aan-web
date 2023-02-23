using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.TermsAndConditionsControllerTests
{
    [TestFixture]
    internal class TermsAndConditionsControllerAttributeTests
    {
        [Test]
        public void Controller_HasCorrectAttributes()
        {
            typeof(TermsAndConditionsController).Should().BeDecoratedWith<RouteAttribute>();
            typeof(TermsAndConditionsController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/terms-and-conditions");
            typeof(TermsAndConditionsController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be(RouteNames.Onboarding.TermsAndConditions);
        }
    }
}
