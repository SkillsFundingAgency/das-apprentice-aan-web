using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.EditApprenticeshipInformationControllerTests;
public class EditApprenticeshipInformationControllerPostTests
{
    EditApprenticeshipInformationController sut = null!;
    private Mock<IOuterApiClient> outerApiMock = null!;
    private Mock<IValidator<SubmitApprenticeshipInformationModel>> validatorMock = null!;
    private readonly Guid memberId = Guid.NewGuid();
    private readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();
    private SubmitApprenticeshipInformationModel submitApprenticeshipInformationModel = null!;
    private GetMemberProfileResponse getMemberProfileResponse = null!;

    [Test, MoqInlineAutoData]
    public async Task Post_InvalidCommand_ShouldReturnsEditApprenticeshipInformationView(
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpModelValidateFalse();

        //Act
        var response = await sut.Post(submitApprenticeshipInformationModel, cancellationToken);
        var result = response as ViewResult;

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.InstanceOf<ViewResult>());
            Assert.That(result!.ViewName, Does.Contain(SharedRouteNames.EditApprenticeshipInformation));
        });
    }

    [Test, MoqInlineAutoData]
    public async Task Post_InvalidCommand_ShouldInvokeGetMemberProfile(
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpModelValidateFalse();

        //Act
        var response = await sut.Post(submitApprenticeshipInformationModel, cancellationToken);

        //Assert
        outerApiMock.Verify(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test, MoqInlineAutoData]
    public async Task Post_InvalidCommand_ShouldReturnEditApprenticeshipInformationViewModel(
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpModelValidateFalse();

        //Act
        var response = await sut.Post(submitApprenticeshipInformationModel, cancellationToken);
        var result = response as ViewResult;
        var viewModel = result!.Model as EditApprenticeshipInformationViewModel;

        //Assert
        Assert.That(viewModel, Is.InstanceOf<EditApprenticeshipInformationViewModel>());
    }

    [Test, RecursiveMoqAutoData]
    public async Task Post_PostValidCommand_RedirectToYourAmbassadorProfile(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpModelValidateTrue();

        // Act
        var response = await sut.Post(submitApprenticeshipInformationModel, cancellationToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.TypeOf<RedirectToRouteResult>());
            var redirectToAction = (RedirectToRouteResult)response;
            Assert.That(redirectToAction.RouteName, Does.Contain(SharedRouteNames.YourAmbassadorProfile));
        });
    }

    [Test, RecursiveMoqAutoData]
    public async Task Index_PostValidCommand_TempDataValueIsSet(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpModelValidateTrue();

        // Act
        var response = await sut.Post(submitApprenticeshipInformationModel, cancellationToken);

        // Assert
        Assert.That(sut.TempData.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage), Is.EqualTo(true));
    }

    [Test, RecursiveMoqAutoData]
    public async Task Index_PostValidCommand_ShouldInvokeUpdateMemberProfileAndPreferences(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpModelValidateTrue();

        // Act
        await sut.Post(submitApprenticeshipInformationModel, cancellationToken);

        // Assert
        outerApiMock.Verify(a => a.UpdateMemberProfileAndPreferences(It.IsAny<Guid>(), It.IsAny<UpdateMemberProfileAndPreferencesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestCase("employername  ", "employername")]
    [TestCase("employername", "employername")]
    [TestCase("  employername", "employername")]
    [TestCase("", "")]
    [TestCase(null, null)]
    public async Task Post_SetsEmployerName(string? employerName, string? expectedEmployerName)
    {
        // Arrange
        SetUpModelValidateTrue();
        submitApprenticeshipInformationModel.EmployerName = employerName;

        //Act
        await sut.Post(submitApprenticeshipInformationModel, CancellationToken.None);

        //Assert
        outerApiMock.Verify(a => a.UpdateMemberProfileAndPreferences(
        It.IsAny<Guid>(),
            It.Is<UpdateMemberProfileAndPreferencesRequest>(m => m.UpdateMemberProfileRequest.MemberProfiles.ElementAt(0).Value == expectedEmployerName && m.PatchMemberRequest.OrganisationName == expectedEmployerName),
            It.IsAny<CancellationToken>()));
    }

    [TestCase("employeraddress1  ", "employeraddress1")]
    [TestCase("employeraddress1", "employeraddress1")]
    [TestCase("  employeraddress1", "employeraddress1")]
    [TestCase("", "")]
    [TestCase(null, null)]
    public async Task Post_SetsEmployerAddress1(string? employerAddress1, string? expectedEmployerAddress1)
    {
        // Arrange
        SetUpModelValidateTrue();
        submitApprenticeshipInformationModel.EmployerAddress1 = employerAddress1;

        //Act
        await sut.Post(submitApprenticeshipInformationModel, CancellationToken.None);

        //Assert
        outerApiMock.Verify(a => a.UpdateMemberProfileAndPreferences(
            It.IsAny<Guid>(),
            It.Is<UpdateMemberProfileAndPreferencesRequest>(m => m.UpdateMemberProfileRequest.MemberProfiles.ElementAt(1).Value == expectedEmployerAddress1),
            It.IsAny<CancellationToken>()));
    }

    [TestCase("employeraddress2  ", "employeraddress2")]
    [TestCase("employeraddress2", "employeraddress2")]
    [TestCase("  employeraddress2", "employeraddress2")]
    [TestCase("", "")]
    [TestCase(null, null)]
    public async Task Post_SetsEmployerAddress2(string? employerAddress2, string? expectedEmployerAddress2)
    {
        // Arrange
        SetUpModelValidateTrue();
        submitApprenticeshipInformationModel.EmployerAddress2 = employerAddress2;

        //Act
        await sut.Post(submitApprenticeshipInformationModel, CancellationToken.None);

        //Assert
        outerApiMock.Verify(a => a.UpdateMemberProfileAndPreferences(
            It.IsAny<Guid>(),
            It.Is<UpdateMemberProfileAndPreferencesRequest>(m => m.UpdateMemberProfileRequest.MemberProfiles.ElementAt(2).Value == expectedEmployerAddress2),
            It.IsAny<CancellationToken>()));
    }

    [TestCase("employercounty  ", "employercounty")]
    [TestCase("employercounty", "employercounty")]
    [TestCase("  employercounty", "employercounty")]
    [TestCase("", "")]
    [TestCase(null, null)]
    public async Task Post_SetsEmployerCounty(string? employerCounty, string? expectedEmployerCounty)
    {
        // Arrange
        SetUpModelValidateTrue();
        submitApprenticeshipInformationModel.EmployerCounty = employerCounty;

        //Act
        await sut.Post(submitApprenticeshipInformationModel, CancellationToken.None);

        //Assert
        outerApiMock.Verify(a => a.UpdateMemberProfileAndPreferences(
            It.IsAny<Guid>(),
            It.Is<UpdateMemberProfileAndPreferencesRequest>(m => m.UpdateMemberProfileRequest.MemberProfiles.ElementAt(3).Value == expectedEmployerCounty),
            It.IsAny<CancellationToken>()));
    }

    [TestCase("employertownorcity  ", "employertownorcity")]
    [TestCase("employertownorcity", "employertownorcity")]
    [TestCase("  employertownorcity", "employertownorcity")]
    [TestCase("", "")]
    [TestCase(null, null)]
    public async Task Post_SetsEmployerTownOrCity(string? employerTownOrCity, string? expectedEmployerTownOrCity)
    {
        // Arrange
        SetUpModelValidateTrue();
        submitApprenticeshipInformationModel.EmployerTownOrCity = employerTownOrCity;

        //Act
        await sut.Post(submitApprenticeshipInformationModel, CancellationToken.None);

        //Assert
        outerApiMock.Verify(a => a.UpdateMemberProfileAndPreferences(
            It.IsAny<Guid>(),
            It.Is<UpdateMemberProfileAndPreferencesRequest>(m => m.UpdateMemberProfileRequest.MemberProfiles.ElementAt(4).Value == expectedEmployerTownOrCity),
            It.IsAny<CancellationToken>()));
    }

    [TestCase("employerpostcode  ", "employerpostcode")]
    [TestCase("employerpostcode", "employerpostcode")]
    [TestCase("  employerpostcode", "employerpostcode")]
    [TestCase("", "")]
    [TestCase(null, null)]
    public async Task Post_SetsEmployerPostcode(string? employerPostcode, string? expectedEmployerPostcode)
    {
        // Arrange
        SetUpModelValidateTrue();
        submitApprenticeshipInformationModel.EmployerPostcode = employerPostcode;

        //Act
        await sut.Post(submitApprenticeshipInformationModel, CancellationToken.None);

        //Assert
        outerApiMock.Verify(a => a.UpdateMemberProfileAndPreferences(
            It.IsAny<Guid>(),
            It.Is<UpdateMemberProfileAndPreferencesRequest>(m => m.UpdateMemberProfileRequest.MemberProfiles.ElementAt(5).Value == expectedEmployerPostcode),
            It.IsAny<CancellationToken>()));
    }

    [TearDown]
    public void TearDown()
    {
        if (sut != null) sut.Dispose();
    }

    private void SetUpControllerWithContext()
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut = new EditApprenticeshipInformationController(outerApiMock.Object, validatorMock.Object);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
    }
    private void SetUpModelValidateTrue()
    {
        outerApiMock = new();
        validatorMock = new();
        SetUpControllerWithContext();

        submitApprenticeshipInformationModel = new()
        {
            EmployerName = "EmployerName",
            EmployerAddress1 = "EmployerAddress1",
            EmployerAddress2 = "EmployerAddress2",
            EmployerCounty = "EmployerCounty",
            EmployerTownOrCity = "EmployerTownOrCity",
            EmployerPostcode = "EmployerPostcode"
        };

        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<SubmitApprenticeshipInformationModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
    }

    private void SetUpModelValidateFalse()
    {
        outerApiMock = new();
        validatorMock = new();
        SetUpControllerWithContext();

        submitApprenticeshipInformationModel = new()
        {
            EmployerName = string.Empty,
            EmployerAddress1 = string.Empty,
            EmployerAddress2 = string.Empty,
            EmployerCounty = string.Empty,
            EmployerTownOrCity = string.Empty,
            EmployerPostcode = string.Empty
        };

        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<SubmitApprenticeshipInformationModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("TestField","Test Message"){ErrorCode = "1001"}
            }));

        getMemberProfileResponse = new()
        {
            Profiles = new List<MemberProfile>(),
            Preferences = new List<MemberPreference>(),
            RegionId = 1,
            OrganisationName = string.Empty
        };
        outerApiMock.Setup(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(getMemberProfileResponse));
    }
}
