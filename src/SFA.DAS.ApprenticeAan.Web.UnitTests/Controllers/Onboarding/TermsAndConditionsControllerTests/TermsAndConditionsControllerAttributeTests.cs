﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Filters;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.TermsAndConditionsControllerTests
{
    [TestFixture]
    internal class TermsAndConditionsControllerAttributeTests
    {
        [Test]
        public void Controller_HasCorrectRouteAttribute()
        {
            typeof(TermsAndConditionsController).Should().BeDecoratedWith<RouteAttribute>();
            typeof(TermsAndConditionsController).Should().BeDecoratedWith<RouteAttribute>().Subject.Template.Should().Be("onboarding/terms-and-conditions");
            typeof(TermsAndConditionsController).Should().BeDecoratedWith<RouteAttribute>().Subject.Name.Should().Be("TermsAndConditions");
        }

        [Test]
        public void Controller_HasRequiredSessionModelAttribute()
        {
            typeof(TermsAndConditionsController).Should().BeDecoratedWith<RequiredSessionModelAttribute>();
        }
    }
}