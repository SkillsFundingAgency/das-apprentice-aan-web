using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.BeforeYouStartControllerTests;

public class BeforeYouStartControllerGetTests
{
    [Test, MoqAutoData]
    public void Get_ReturnsViewResult([Greedy] BeforeYouStartController sut)
    {
        var result = sut.Get();

        result.As<ViewResult>().Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public void Get_ViewResult_HasCorrectViewPath([Greedy] BeforeYouStartController sut)
    {
        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(BeforeYouStartController.ViewPath);
    }
}
