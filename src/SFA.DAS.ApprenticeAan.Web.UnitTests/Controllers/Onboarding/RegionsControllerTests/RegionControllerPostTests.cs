using AutoFixture.NUnit3;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.RegionsControllerTests;

[TestFixture]
public class RegionControllerPostTests
{
    [MoqAutoData]
    public async Task Post_SetsSelectedRegionInOnBoardingSessionModel(
  [Frozen] Mock<ISessionService> sessionServiceMock,
  [Frozen] Mock<IValidator<RegionSubmitModel>> validatorMock,
  [Greedy] RegionController sut,
  RegionSubmitModel submitmodel)
    {
        sut.AddUrlHelperMock();
        ValidationResult validationResult = new();

        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        await sut.Post(submitmodel);
        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.RegionId == submitmodel.SelectedRegionId)));
    }
}