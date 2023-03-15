using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.RegionsControllerTests;


[TestFixture]
public class RegionsControllerPostTests
{
    [MoqAutoData]
    public async Task Post_SetsSelectedRegionInOnBoardingSessionModel(
   [Frozen] Mock<ISessionService> sessionServiceMock,
   [Greedy] RegionsController sut, [Greedy] RegionsSubmitModel submitmodel)
    {
        OnboardingSessionModel value = new();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(value);

        submitmodel.SelectedRegionId = 10;

        var result = await sut.Post(submitmodel);
        result.As<ViewResult>().Model.As<RegionsViewModel>().SelectedRegionId.Should().Be(submitmodel.SelectedRegionId);
        result.As<RedirectToRouteResult>().RouteName.Should().Be("Regions");

    }
}
