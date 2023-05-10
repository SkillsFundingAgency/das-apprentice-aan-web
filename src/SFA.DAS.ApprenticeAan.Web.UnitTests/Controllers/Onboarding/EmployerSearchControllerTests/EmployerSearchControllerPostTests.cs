using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
            m.GetProfileValue(ProfileDataId.EmployerName) == submitModel.OrganisationName &&
            m.GetProfileValue(ProfileDataId.AddressLine1) == submitModel.AddressLine1 &&
            m.GetProfileValue(ProfileDataId.AddressLine2) == submitModel.AddressLine2 &&
            m.GetProfileValue(ProfileDataId.County) == submitModel.County &&
            m.GetProfileValue(ProfileDataId.Town) == submitModel.Town &&
            m.GetProfileValue(ProfileDataId.Postcode) == submitModel.Postcode
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
                new ProfileModel { Id = ProfileDataId.EmployerName },
                new ProfileModel { Id = ProfileDataId.AddressLine1 },
                new ProfileModel { Id = ProfileDataId.AddressLine2 },
                new ProfileModel { Id = ProfileDataId.County },
                new ProfileModel { Id = ProfileDataId.Town },
                new ProfileModel { Id = ProfileDataId.Postcode },
                new ProfileModel { Id = ProfileDataId.Longitude },
                new ProfileModel { Id = ProfileDataId.Latitude }
            }
        };
}
