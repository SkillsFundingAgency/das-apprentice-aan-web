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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.BeforeYouStartControllerTests;

[TestFixture]
public class CurrentJobTitleControllerPostTests
{
    [MoqAutoData]
    public void Post_SetsEnteredJobTitleInOnBoardingSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<CurrentJobTitleSubmitModel>> validatorMock,
        [Greedy] CurrentJobTitleController sut,
        CurrentJobTitleSubmitModel submitmodel)
    {
        sut.AddUrlHelperMock();

        ValidationResult validationResult = new();
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        sut.Post(submitmodel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.ProfileData.FirstOrDefault(x => x.Id == 20)!.Value == submitmodel.EnteredJobTitle)));
    }

    [MoqAutoData]
    public void Post_Errors_WhenEnteredJobTitleIsNull(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CurrentJobTitleController sut)
    {
        sut.AddUrlHelperMock();
        CurrentJobTitleSubmitModel submitmodel = new();
        submitmodel.EnteredJobTitle = null;

        var result = sut.Post(submitmodel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.ProfileData.FirstOrDefault(x => x.Id == 20)!.Value == null)));
        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => !m.IsValid)));
        result.As<ViewResult>().Model.As<CurrentJobTitleSubmitModel>().EnteredJobTitle.Should().BeNull();
    }
}