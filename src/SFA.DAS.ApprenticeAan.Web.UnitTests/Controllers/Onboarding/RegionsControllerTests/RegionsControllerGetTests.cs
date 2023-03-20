using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.RegionsControllerTests;

[TestFixture]
public class RegionsControllerGetTests
{
    [MoqAutoData]
    public async Task Get_ReturnsViewResult([Greedy] RegionsController sut)
    {
        sut.AddUrlHelperMock();
        var result = await sut.Get();

        result.As<ViewResult>().Should().NotBeNull();
    }

    [MoqAutoData]
    public async Task Get_ViewResult_HasCorrectViewPath([Greedy] RegionsController sut)
    {
        sut.AddUrlHelperMock();
        var result = await sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(RegionsController.ViewPath);
    }

    [MoqAutoData]
    public async Task Get_ViewModel_HasBackLink(
        [Greedy] RegionsController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);
        var result = await sut.Get();

        result.As<ViewResult>().Model.As<RegionsViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }
}