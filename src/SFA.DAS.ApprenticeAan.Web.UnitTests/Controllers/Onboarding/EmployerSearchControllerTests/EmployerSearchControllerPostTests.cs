using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.ApprenticeAan.Domain.Constants;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.EmployerSearchControllerTests;

public class EmployerSearchControllerPostTests
{
    [Test]
    [MoqAutoData]
    public void Post_InvalidModel_ReturnsView(
        [Greedy] EmployerSearchController sut)
    {
        sut.AddUrlHelperMock();
        var result = sut.Post(new EmployerSearchSubmitModel());

        result.As<ViewResult>().ViewData.ModelState.IsValid.Should().BeFalse();
    }

    [Test]
    [MoqAutoData]
    public void Post_AddressSelected_UpdateSessionModelWithSubmittedValues(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<EmployerSearchSubmitModel>> validatorMock,
        [Greedy] EmployerSearchController sut,
        EmployerSearchSubmitModel submitModel)
    {
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(GetSessionModelWithProfile());
        sut.AddUrlHelperMock();

        sut.Post(submitModel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m =>
            m.GetProfileValue(ProfileConstants.ProfileIds.EmployerName) == submitModel.OrganisationName &&
            m.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddress1) == submitModel.AddressLine1 &&
            m.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddress2) == submitModel.AddressLine2 &&
            m.GetProfileValue(ProfileConstants.ProfileIds.EmployerCounty) == submitModel.County &&
            m.GetProfileValue(ProfileConstants.ProfileIds.EmployerTownOrCity) == submitModel.Town &&
            m.GetProfileValue(ProfileConstants.ProfileIds.EmployerPostcode) == submitModel.Postcode &&
            m.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddressLongitude) == submitModel.Longitude.ToString() &&
            m.GetProfileValue(ProfileConstants.ProfileIds.EmployerAddressLatitude) == submitModel.Latitude.ToString()
        )));
    }

    [Test]
    [MoqAutoData]
    public void Post_AddressSelected_RedirectToEmployerDetailsToConfirmAddress(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<EmployerSearchSubmitModel>> validatorMock,
        [Greedy] EmployerSearchController sut,
        EmployerSearchSubmitModel submitModel)
    {
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(GetSessionModelWithProfile());
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());
        sut.AddUrlHelperMock();

        var result = sut.Post(submitModel);

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.EmployerDetails);
    }

    private static OnboardingSessionModel GetSessionModelWithProfile() =>
        new()
        {
            ProfileData = new()
            {
                new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerName },
                new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerAddress1 },
                new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerAddress2 },
                new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerCounty },
                new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerTownOrCity },
                new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerPostcode },
                new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerAddressLongitude },
                new ProfileModel { Id = ProfileConstants.ProfileIds.EmployerAddressLatitude }
            }
        };
}
