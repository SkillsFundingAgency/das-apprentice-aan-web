using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.AreasOfInterestControllerTests;

[TestFixture]
public class AreasOfInterestControllerGetTests
{
    [MoqAutoData]
    public void Get_ReturnsViewResult([Greedy] AreasOfInterestController sut)
    {
        sut.AddUrlHelperMock();
        var result = sut.Get();

        result.As<ViewResult>().Should().NotBeNull();
    }

    [MoqAutoData]
    public void Get_ViewResult_HasCorrectViewPath([Greedy] AreasOfInterestController sut)
    {
        sut.AddUrlHelperMock();
        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(AreasOfInterestController.ViewPath);
    }

    [MoqAutoData]
    public void Get_ViewModel_GetsAreasOfInterestViewModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        OnboardingSessionModel sessionModel,
        [Greedy] AreasOfInterestController sut)
    {
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.ReasonToJoin);

        sessionModel.ProfileData.Add(new ProfileModel { Id = 101, Category = "Events", Value = true.ToString() });
        sessionModel.ProfileData.Add(new ProfileModel { Id = 202, Category = "Promotions", Value = false.ToString() });

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        var result = sut.Get();

        result.As<ViewResult>().Model.As<AreasOfInterestViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
        result.As<ViewResult>().Model.As<AreasOfInterestViewModel>().AreasOfInterest.Should().Contain(x => x.Id == 101 && x.Category == "Events" && x.IsSelected);
        result.As<ViewResult>().Model.As<AreasOfInterestViewModel>().AreasOfInterest.Should().Contain(x => x.Id == 202 && x.Category == "Promotions" && !x.IsSelected);
    }
}