using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.LineManagerControllerTests;

[TestFixture]
public class LineManagerControllerGetTests
{
    [MoqAutoData]
    public void Get_ReturnsViewResult([Greedy] LineManagerController sut)
    {
        sut.AddUrlHelperMock();
        var result = sut.Get();

        result.As<ViewResult>().Should().NotBeNull();
    }

    [MoqAutoData]
    public void Get_ViewResult_HasCorrectViewPath([Greedy] LineManagerController sut)
    {
        sut.AddUrlHelperMock();
        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(LineManagerController.ViewPath);
    }

    [MoqAutoData]
    public void Get_ViewModel_HasBackLink([Greedy] LineManagerController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.TermsAndConditions);
        var result = sut.Get();

        result.As<ViewResult>().Model.As<LineManagerViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }
}
