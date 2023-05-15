using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestEase;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.EmployerDetailsControllerTests;

[TestFixture]
public class EmployerDetailsControllerPostTests
{
    const string Category = "Employer";

    OnboardingSessionModel sessionModel;
    Mock<ISessionService> sessionServiceMock;
    Mock<EmployerDetailsSubmitModel> submitModel;
    ValidationResult validationResult;
    Mock<IValidator<EmployerDetailsSubmitModel>> validatorMock;
    Response<GetCoordinatesResult> response;
    Mock<IOuterApiClient> outerApiClient;
    EmployerDetailsController sut;

    [SetUp]
    public void Init()
    {
        sessionModel = new();
        sessionServiceMock = new();
        submitModel = new();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock = new();
        outerApiClient = new();

        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.EmployerName, Description = "Employer name", Category = Category, Ordering = 1 });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.AddressLine1, Description = "Employer Address line 1", Category = Category, Ordering = 2 });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.AddressLine2, Description = "Employer Address line 2", Category = Category, Ordering = 3 });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.Town, Description = "Employer Town or City", Category = Category, Ordering = 4 });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.County, Description = "Employer County", Category = Category, Ordering = 5 });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.Postcode, Description = "Employer Postcode", Category = Category, Ordering = 6 });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.Longitude, Description = "Employer address longitude", Category = Category, Ordering = 7 });
        sessionModel.ProfileData.Add(new ProfileModel { Id = ProfileDataId.Latitude, Description = "Employer address latitude", Category = Category, Ordering = 8 });

        SetResponseForGetCoordinatesAPI(System.Net.HttpStatusCode.OK);

        validationResult = new();
        validatorMock.Setup(v => v.Validate(submitModel.Object)).Returns(validationResult);

        sut = new(sessionServiceMock.Object, validatorMock.Object, outerApiClient.Object);
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.EmployerSearch);

    }
    [Test]
    public async Task Post_VerifiesSessionModel()
    {
        await sut.PostEmployerDetails(submitModel.Object);

        sessionServiceMock.Verify(s => s.Set(sessionModel));
    }

    [Test]
    public async Task Post_ModelStateIsInvalid_ReloadsViewWithValidationErrors()
    {
        validationResult = new(new List<ValidationFailure>() { new ValidationFailure("key", "message") });
        validatorMock.Setup(v => v.Validate(submitModel.Object)).Returns(validationResult);

        sut = new(sessionServiceMock.Object, validatorMock.Object, outerApiClient.Object);
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.EmployerSearch);

        var result = await sut.PostEmployerDetails(submitModel.Object);

        sut.ModelState.IsValid.Should().BeFalse();
        result.As<ViewResult>().Should().NotBeNull();
        result.As<ViewResult>().ViewName.Should().Be(EmployerDetailsController.ViewPath);
        result.As<ViewResult>().Model.As<EmployerDetailsViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [Test]
    public async Task Post_ModelStateIsValid_UpdatesTheSessionModel()
    {
        await sut.PostEmployerDetails(submitModel.Object);

        sessionServiceMock.Verify(s => s.Set(sessionModel));

        sessionModel.ProfileData.Should().NotBeNull();
        sessionModel.ProfileData.Count.Should().BeGreaterThan(0);

        sessionModel.GetProfileValue(ProfileDataId.EmployerName).Should().Be(submitModel.Object.EmployerName);

        sessionModel.GetProfileValue(ProfileDataId.AddressLine1).Should().Be(submitModel.Object.AddressLine1);
        sessionModel.GetProfileValue(ProfileDataId.AddressLine2).Should().Be(submitModel.Object.AddressLine2);

        sessionModel.GetProfileValue(ProfileDataId.Town).Should().Be(submitModel.Object.Town);
        sessionModel.GetProfileValue(ProfileDataId.County).Should().Be(submitModel.Object.County);

        sessionModel.GetProfileValue(ProfileDataId.Postcode).Should().Be(submitModel.Object.Postcode);

        sut.ModelState.IsValid.Should().BeTrue();
    }


    [Test]
    public async Task Post_ModelStateIsValid_CoordinatesNotFound_SetsCoordinatesInSessionModelToNull()
    {
        SetResponseForGetCoordinatesAPI(System.Net.HttpStatusCode.NotFound);

        await sut.PostEmployerDetails(submitModel.Object);

        sessionModel.GetProfileValue(ProfileDataId.Longitude).Should().Be(null);
        sessionModel.GetProfileValue(ProfileDataId.Latitude).Should().Be(null);
    }

    [Test]
    public async Task Post_ModelStateIsValid_CoordinatesFound_SetsCoordinatesInSessionModel()
    {
        await sut.PostEmployerDetails(submitModel.Object);

        sessionModel.GetProfileValue(ProfileDataId.Longitude).Should().Be(double.MinValue.ToString());
        sessionModel.GetProfileValue(ProfileDataId.Latitude).Should().Be(double.MaxValue.ToString());
    }

    [Test]
    public async Task Post_ModelStateIsValidAndHasNotSeenPreview_RedirectsToCurrentJobTitleView()
    {
        sessionModel.HasSeenPreview = false;

        var result = await sut.PostEmployerDetails(submitModel.Object);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CurrentJobTitle);
    }

    [Test]
    public async Task Post_ModelStateIsValidAndHasSeenPreview_RedirectsToCheckYourAnswers()
    {
        sessionModel.HasSeenPreview = true;

        var result = await sut.PostEmployerDetails(submitModel.Object);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<RedirectToRouteResult>().Should().NotBeNull();
        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.CheckYourAnswers);
    }
    private void SetResponseForGetCoordinatesAPI(System.Net.HttpStatusCode httpStatusCodeForResponse)
    {
        response = new(string.Empty, new HttpResponseMessage(httpStatusCodeForResponse), () => new GetCoordinatesResult() { Longitude = double.MinValue, Latitude = double.MaxValue });
        response.ResponseMessage.StatusCode = httpStatusCodeForResponse;
        outerApiClient.Setup(x => x.GetCoordinates(submitModel.Object.Postcode!)).ReturnsAsync(response);
    }

    [TearDown]
    public void Dispose()
    {
        sessionModel = null!;
        sessionServiceMock = null!;
        submitModel = null!;
        validationResult = null!;
        validatorMock = null!;
        response = null!;
        outerApiClient = null!;
        sut = null!;
    }
}
