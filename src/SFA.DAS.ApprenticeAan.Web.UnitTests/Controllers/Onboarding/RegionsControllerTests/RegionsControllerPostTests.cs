using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
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
        [Frozen] Mock<IValidator<RegionsSubmitModel>> validatorMock,
        [Greedy] RegionsController sut,
        RegionsSubmitModel submitmodel)
    {
        sut.AddUrlHelperMock();

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
        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => !m.IsValid)));
        result.As<ViewResult>().Model.As<RegionsViewModel>().SelectedRegionId.Should().BeNull();
    }
}