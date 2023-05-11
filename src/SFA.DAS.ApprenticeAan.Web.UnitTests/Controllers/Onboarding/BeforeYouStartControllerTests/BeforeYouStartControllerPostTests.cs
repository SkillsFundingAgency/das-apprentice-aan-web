using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.BeforeYouStartControllerTests;

public class BeforeYouStartControllerPostTests
{
    [Test, MoqAutoData]
    public void Post_SessionModel_RedirectsRouteToTermsAndConditions(
         [Greedy] BeforeYouStartController sut)
    {
        var result = sut.Post();

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.TermsAndConditions);
    }
}