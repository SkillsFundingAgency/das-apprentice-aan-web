using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.BeforeYouStartControllerTests;

[TestFixture]
public class BeforeYouStartControllerGetTests
{
    [MoqAutoData]
    public void Get_ReturnsViewResult([Greedy] BeforeYouStartController sut)
    {
        var result = sut.Get();

        result.As<ViewResult>().Should().NotBeNull();
    }

    [MoqAutoData]
    public void Get_ViewResult_HasCorrectViewPath([Greedy] BeforeYouStartController sut)
    {
        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(BeforeYouStartController.ViewPath);
    }

    [MoqAutoData]
    public void Get_ViewModel_HasBackLink(
        [Frozen] ApplicationConfiguration applicationConfiguration,
        [Greedy] BeforeYouStartController sut)
    {
        var result = sut.Get();

        result.As<ViewResult>().Model.As<BeforeYouStartViewModel>().BackLink.Should().Be(applicationConfiguration.ApplicationUrls.ApprenticeHomeUrl.ToString());
    }
}
