using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
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

        var sessionModel = new OnboardingSessionModel();
        sessionModel.ProfileData = new List<ProfileModel> { new ProfileModel() { Id = ProfileDataId.JobTitle, Description = "Job title", Category = "Personal", Ordering = 1, Value = "something" } };
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        ValidationResult validationResult = new();
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        sut.Post(submitmodel);

        sessionServiceMock.Equals(sessionModel.ProfileData.GetProfileModelValue(ProfileDataId.JobTitle) == submitmodel.EnteredJobTitle);
    }

    [MoqAutoData]
    public void Post_Errors_WhenEnteredJobTitleIsNull(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] CurrentJobTitleController sut)
    {
        sut.AddUrlHelperMock();

        var sessionModel = new OnboardingSessionModel();
        sessionModel.ProfileData = new List<ProfileModel> { new ProfileModel() { Id = ProfileDataId.JobTitle, Description = "Job title", Category = "Personal", Ordering = 1, Value = "something" } };
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        CurrentJobTitleSubmitModel submitmodel = new();
        submitmodel.EnteredJobTitle = null;

        var result = sut.Post(submitmodel);

        sessionServiceMock.Equals(sessionModel.ProfileData.GetProfileModelValue(ProfileDataId.JobTitle) == null);
        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => !m.IsValid)));
        result.As<ViewResult>().Model.As<CurrentJobTitleSubmitModel>().EnteredJobTitle.Should().BeNull();
    }
}