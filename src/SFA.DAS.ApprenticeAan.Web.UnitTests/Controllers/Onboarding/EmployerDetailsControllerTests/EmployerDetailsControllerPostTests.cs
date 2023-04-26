using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.EmployerDetailsControllerTests;

[TestFixture]
public class EmployerDetailsControllerPostTests
{
    const string userType = "apprentice";
    const string Category = "Employer";

    private readonly List<Profile> _profiles;
    public EmployerDetailsControllerPostTests()
    {
        _profiles = new List<Profile>
        {
            new() { Id = ProfileDataId.EmployerName, Description = "Employer name", Category = Category, Ordering = 1},
            new() { Id = ProfileDataId.AddressLine1, Description = "Employer Address line 1", Category = Category, Ordering = 2 },
            new() { Id = ProfileDataId.AddressLine2, Description = "Employer Address line 2", Category = Category, Ordering = 3 },
            new() { Id = ProfileDataId.Town, Description = "Employer Town or City", Category = Category, Ordering = 4 },
            new() { Id = ProfileDataId.County, Description = "Employer County", Category = Category, Ordering = 5 },
            new() { Id = ProfileDataId.Postcode, Description = "Employer Postcode", Category = Category, Ordering = 6 }
        };
    }

    [MoqAutoData]
    public void Post_ModelStateIsInvalid_ReloadsViewWithValidationErrors(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IProfileService> profileServiceMock,
        [Frozen] EmployerDetailsSubmitModel submitmodel,
        [Greedy] EmployerDetailsController sut)
    {
        OnboardingSessionModel sessionModel = new();
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.EmployerSearch);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData = _profiles.Select(p => (ProfileModel)p).ToList();

        profileServiceMock.Setup(s => s.GetProfilesByUserType(userType)).ReturnsAsync(_profiles);

        sut.ModelState.AddModelError("key", "message");

        var result = sut.PostEmployerDetails(submitmodel);

        sut.ModelState.IsValid.Should().BeFalse();

        result.As<ViewResult>().Should().NotBeNull();
        result.As<ViewResult>().ViewName.Should().Be(EmployerDetailsController.ViewPath);
        result.As<ViewResult>().Model.As<EmployerDetailsViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Post_ModelStateIsValid_UpdatesSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IProfileService> profileServiceMock,
        [Frozen] Mock<IValidator<EmployerDetailsSubmitModel>> validatorMock,
        [Frozen] EmployerDetailsSubmitModel submitmodel,
        [Greedy] EmployerDetailsController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.TermsAndConditions);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData = _profiles.Select(p => (ProfileModel)p).ToList();
        profileServiceMock.Setup(s => s.GetProfilesByUserType(userType)).ReturnsAsync(_profiles);

        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        sessionServiceMock.Object.Set(sessionModel);

        sut.PostEmployerDetails(submitmodel);

        sessionServiceMock.Verify(s => s.Set(sessionModel));

        sessionModel.ProfileData.Should().NotBeNull();
        sessionModel.ProfileData.Count.Should().BeGreaterThan(0);

        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.EmployerName)?.Value.Should().NotBe(null);
        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.EmployerName)?.Value.Should().Be(submitmodel.EmployerName);

        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.AddressLine1)?.Value.Should().NotBe(null);
        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.AddressLine1)?.Value.Should().Be(submitmodel.AddressLine1);
        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.AddressLine2)?.Value.Should().Be(submitmodel.AddressLine2);

        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.Town)?.Value.Should().NotBe(null);
        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.Town)?.Value.Should().Be(submitmodel.Town);
        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.County)?.Value.Should().Be(submitmodel.County);

        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.Postcode)?.Value.Should().NotBe(null);
        sessionModel.ProfileData.FirstOrDefault(p => p.Id == ProfileDataId.Postcode)?.Value.Should().Be(submitmodel.Postcode);

        sut.ModelState.IsValid.Should().BeTrue();
    }

    [MoqAutoData]
    public void Post_ModelStateIsValidAndHasNotSeenPreview_RedirectsToCurrentJobTitleView(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IProfileService> profileServiceMock,
        [Frozen] Mock<IValidator<EmployerDetailsSubmitModel>> validatorMock,
        [Frozen] EmployerDetailsSubmitModel submitmodel,
        [Greedy] EmployerDetailsController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.LineManager);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData = _profiles.Select(p => (ProfileModel)p).ToList();
        sessionModel.HasSeenPreview = false;
        profileServiceMock.Setup(s => s.GetProfilesByUserType(userType)).ReturnsAsync(_profiles);

        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        var result = sut.PostEmployerDetails(submitmodel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CurrentJobTitle);
    }

    [MoqAutoData]
    public void Post_ModelStateIsValidAndHasSeenPreview_RedirectsToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IProfileService> profileServiceMock,
        [Frozen] Mock<IValidator<EmployerDetailsSubmitModel>> validatorMock,
        [Frozen] EmployerDetailsSubmitModel submitmodel,
        [Greedy] EmployerDetailsController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.LineManager);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        sessionModel.ProfileData = _profiles.Select(p => (ProfileModel)p).ToList();
        sessionModel.HasSeenPreview = true;
        profileServiceMock.Setup(s => s.GetProfilesByUserType(userType)).ReturnsAsync(_profiles);

        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        var result = sut.PostEmployerDetails(submitmodel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }
}
