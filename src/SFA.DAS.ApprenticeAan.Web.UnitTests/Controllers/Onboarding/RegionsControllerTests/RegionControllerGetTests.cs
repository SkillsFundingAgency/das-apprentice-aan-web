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
public class RegionControllerGetTests
{
    [MoqAutoData]
    public async Task Get_ReturnsViewResult([Greedy] RegionController sut)
    {
        sut.AddUrlHelperMock();
        var result = await sut.Get();

        result.As<ViewResult>().Should().NotBeNull();
    }

    [MoqAutoData]
    public async Task Get_ViewResult_HasCorrectViewPath([Greedy] RegionController sut)
    {
        sut.AddUrlHelperMock();
        var result = await sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(RegionController.ViewPath);
    }

    [MoqAutoData]
    public async Task Get_ViewModel_HasBackLink(
        [Greedy] RegionController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);
        var result = await sut.Get();

        result.As<ViewResult>().Model.As<RegionViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }
}