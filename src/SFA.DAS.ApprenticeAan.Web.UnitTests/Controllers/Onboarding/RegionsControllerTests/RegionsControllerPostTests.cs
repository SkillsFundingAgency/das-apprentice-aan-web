using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.RegionsControllerTests;

[TestFixture]
public class RegionsControllerPostTests
{
    [MoqAutoData]
    public async Task Post_SetsSelectedRegionInOnBoardingSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IRegionsService> regionsService,
        [Frozen] Mock<IValidator<RegionsSubmitModel>> validatorMock,
        [Greedy] RegionsController sut,
        RegionsSubmitModel submitmodel
        )
    {
        sut.AddUrlHelperMock();

        List<Region> regionList = new()
        {
            new Region() { Area = "London", Id = (int)submitmodel.SelectedRegionId!, Ordering = 1 }
        };

        regionsService.Setup(x => x.GetRegions()).Returns(Task.FromResult(regionList));

        ValidationResult validationResult = new();
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        await sut.Post(submitmodel);
        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.RegionId == submitmodel.SelectedRegionId)));
    }

    [MoqAutoData]
    public async Task Post_Errors_WhenSelectedRegionIsNull(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] RegionsController sut)
    {
        sut.AddUrlHelperMock();
        RegionsSubmitModel submitmodel = new()
        {
            SelectedRegionId = null
        };

        var result = await sut.Post(submitmodel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.RegionId == null)));
        result.As<ViewResult>().Model.As<RegionsViewModel>().SelectedRegionId.Should().BeNull();
    }

    [MoqAutoData]
    public async Task Post_IsValidAndHasNotSeenPreview_RedirectsToReasonToJoinView(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IRegionsService> regionsService,
        [Frozen] Mock<IValidator<RegionsSubmitModel>> validatorMock,
        [Frozen] RegionsSubmitModel submitmodel,
        [Greedy] RegionsController sut,
        OnboardingSessionModel sessionModel)
    {
        ValidationResult validationResult = new();
        sessionModel.HasSeenPreview = false;

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);

        List<Region> regionList = new()
        {
            new Region() { Area = "London", Id = (int)submitmodel.SelectedRegionId!, Ordering = 1 }
        };

        regionsService.Setup(x => x.GetRegions()).Returns(Task.FromResult(regionList));
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        var result = await sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.ReasonToJoin);
    }

    [MoqAutoData]
    public async Task Post_IsValidAndHasSeenPreview_RedirectsToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IRegionsService> regionsService,
        [Frozen] Mock<IValidator<RegionsSubmitModel>> validatorMock,
        [Frozen] RegionsSubmitModel submitmodel,
        [Greedy] RegionsController sut,
        OnboardingSessionModel sessionModel)
    {
        ValidationResult validationResult = new();
        sessionModel.HasSeenPreview = true;

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);

        List<Region> regionList = new()
        {
            new Region() { Area = "London", Id = (int)submitmodel.SelectedRegionId!, Ordering = 1 }
        };

        regionsService.Setup(x => x.GetRegions()).Returns(Task.FromResult(regionList));
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        var result = await sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }
}