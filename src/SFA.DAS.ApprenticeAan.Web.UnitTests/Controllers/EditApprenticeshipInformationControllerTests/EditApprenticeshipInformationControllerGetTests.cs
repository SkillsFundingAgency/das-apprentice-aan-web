using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.EditApprenticeshipInformation;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.Aan.SharedUi.Constants.PreferenceConstants;
using static SFA.DAS.Aan.SharedUi.Constants.ProfileConstants;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.EditApprenticeshipInformationControllerTests;
public class EditApprenticeshipInformationControllerGetTests
{
    EditApprenticeshipInformationController sut = null!;
    private Mock<IOuterApiClient> outerApiMock = null!;
    private Mock<IValidator<SubmitApprenticeshipInformationModel>> validatorMock = null!;
    private readonly Guid memberId = Guid.NewGuid();
    private readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();
    private GetMemberProfileResponse getMemberProfileResponse = null!;

    [Test, MoqAutoData]
    public void Index_ShouldReturnEditApprenticeshipInformationView(
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.Index(cancellationToken);
        var viewResult = result as ViewResult;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.That(viewResult!.ViewName, Does.Contain(SharedRouteNames.EditApprenticeshipInformation));
        });
    }

    [Test, MoqAutoData]
    public void Index_ShouldInvokeGetMemberProfile(
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.Index(cancellationToken);

        //Assert
        outerApiMock.Verify(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public void Index_ShouldReturnEditApprenticeshipInformationViewModel(
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditApprenticeshipInformationViewModel;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(viewModel, Is.InstanceOf<EditApprenticeshipInformationViewModel>());
        });
    }

    [Test, RecursiveMoqAutoData]
    public void Index_ApprenticeshipInformationIsNotNull_ExpectedValueShouldReturn(
        string employerName,
        string employerAddress1,
        string employerAddress2,
        string employerTownOrCity,
        string employerCounty,
        string employerPostcode,
        bool showApprenticeshipInformation,
        GetMemberProfileResponse getMemberProfileResponse,
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpOuterApiMock();
        getMemberProfileResponse.Profiles = new List<MemberProfile>()
            {
                new(){ProfileId=ProfileIds.EmployerName,Value=employerName},
                new(){ProfileId=ProfileIds.EmployerAddress1,Value=employerAddress1},
                new(){ProfileId=ProfileIds.EmployerAddress2,Value=employerAddress2},
                new(){ProfileId=ProfileIds.EmployerTownOrCity,Value=employerTownOrCity},
                new(){ProfileId=ProfileIds.EmployerCounty,Value=employerCounty},
                new(){ProfileId=ProfileIds.EmployerPostcode,Value=employerPostcode}
            };
        getMemberProfileResponse.Preferences = new List<MemberPreference>()
            {
                new(){PreferenceId=PreferenceIds.Apprenticeship,Value=showApprenticeshipInformation}
            };
        outerApiMock.Setup(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(getMemberProfileResponse));

        // Act
        var result = sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditApprenticeshipInformationViewModel;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(viewModel!.EmployerName, Is.EqualTo(employerName));
            Assert.That(viewModel!.EmployerAddress1, Is.EqualTo(employerAddress1));
            Assert.That(viewModel!.EmployerAddress2, Is.EqualTo(employerAddress2));
            Assert.That(viewModel!.EmployerTownOrCity, Is.EqualTo(employerTownOrCity));
            Assert.That(viewModel!.EmployerCounty, Is.EqualTo(employerCounty));
            Assert.That(viewModel!.EmployerPostcode, Is.EqualTo(employerPostcode));
            Assert.That(viewModel!.ShowApprenticeshipInformation, Is.EqualTo(showApprenticeshipInformation));
            Assert.That(viewModel!.Sector, Is.EqualTo(getMemberProfileResponse.Apprenticeship!.Sector));
            Assert.That(viewModel!.Programmes, Is.EqualTo(getMemberProfileResponse.Apprenticeship!.Programme));
            Assert.That(viewModel!.Level, Is.EqualTo(getMemberProfileResponse.Apprenticeship!.Level));
        });
    }

    [Test, RecursiveMoqAutoData]
    public void Index_ApprenticeshipInformationIsNull_ExpectedValueShouldReturn(
        string employerName,
        string employerAddress1,
        string employerAddress2,
        string employerTownOrCity,
        string employerCounty,
        string employerPostcode,
        bool showApprenticeshipInformation,
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpOuterApiMock();
        getMemberProfileResponse.Profiles = new List<MemberProfile>()
            {
                new(){ProfileId=ProfileIds.EmployerName,Value=employerName},
                new(){ProfileId=ProfileIds.EmployerAddress1,Value=employerAddress1},
                new(){ProfileId=ProfileIds.EmployerAddress2,Value=employerAddress2},
                new(){ProfileId=ProfileIds.EmployerTownOrCity,Value=employerTownOrCity},
                new(){ProfileId=ProfileIds.EmployerCounty,Value=employerCounty},
                new(){ProfileId=ProfileIds.EmployerPostcode,Value=employerPostcode}
            };
        getMemberProfileResponse.Preferences = new List<MemberPreference>()
            {
                new(){PreferenceId=PreferenceIds.Apprenticeship,Value=showApprenticeshipInformation}
            };
        getMemberProfileResponse.Apprenticeship = null;
        outerApiMock.Setup(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(getMemberProfileResponse));

        // Act
        var result = sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditApprenticeshipInformationViewModel;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(viewModel!.ShowApprenticeshipInformation, Is.EqualTo(showApprenticeshipInformation));
            Assert.That(viewModel!.Sector, Is.EqualTo(null));
            Assert.That(viewModel!.Programmes, Is.EqualTo(null));
            Assert.That(viewModel!.Level, Is.EqualTo(null));
        });
    }

    [Test]
    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public void Index_PassValidApprenticeshipInformationPreference_ShouldReturnExpectedPreferenceValue(
        bool showApprenticeshipInformation,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpOuterApiMock();
        getMemberProfileResponse.Preferences = new List<MemberPreference>() {new()
            {
                PreferenceId = PreferenceIds.Apprenticeship,
                Value = showApprenticeshipInformation
            }};
        outerApiMock.Setup(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(getMemberProfileResponse));

        // Act
        var result = sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditApprenticeshipInformationViewModel;

        // Assert
        Assert.That(viewModel!.ShowApprenticeshipInformation, Is.EqualTo(showApprenticeshipInformation));
    }

    [Test, MoqAutoData]
    public void Index_EditApprenticeshipInformationViewModel_ShouldHaveExpectedValueForYourAmbassadorProfileUrl(
            CancellationToken cancellationToken
        )
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditApprenticeshipInformationViewModel;

        // Assert
        Assert.That(viewModel!.YourAmbassadorProfileUrl, Is.EqualTo(YourAmbassadorProfileUrl));
    }

    [Test, MoqAutoData]
    public void GetEditApprenticeshipInformationViewModel_ShouldInvokeGetMemberProfile(
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.GetEditApprenticeshipInformationViewModel(cancellationToken);

        //Assert
        outerApiMock.Verify(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public void GetEditApprenticeshipInformationViewModel_ShouldReturnEditApprenticeshipInformationViewModel(
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.GetEditApprenticeshipInformationViewModel(cancellationToken).Result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<EditApprenticeshipInformationViewModel>());
        });
    }

    [Test, RecursiveMoqAutoData]
    public void GetEditApprenticeshipInformationViewModel_ApprenticeshipInformationIsNotNull_ExpectedValueShouldReturn(
        string employerName,
        string employerAddress1,
        string employerAddress2,
        string employerTownOrCity,
        string employerCounty,
        string employerPostcode,
        bool showApprenticeshipInformation,
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpOuterApiMock();
        getMemberProfileResponse.Profiles = new List<MemberProfile>()
            {
                new(){ProfileId=ProfileIds.EmployerName,Value=employerName},
                new(){ProfileId=ProfileIds.EmployerAddress1,Value=employerAddress1},
                new(){ProfileId=ProfileIds.EmployerAddress2,Value=employerAddress2},
                new(){ProfileId=ProfileIds.EmployerTownOrCity,Value=employerTownOrCity},
                new(){ProfileId=ProfileIds.EmployerCounty,Value=employerCounty},
                new(){ProfileId=ProfileIds.EmployerPostcode,Value=employerPostcode}
            };
        getMemberProfileResponse.Preferences = new List<MemberPreference>()
            {
                new(){PreferenceId=PreferenceIds.Apprenticeship,Value=showApprenticeshipInformation}
            };

        getMemberProfileResponse.Apprenticeship = new()
        {
            Sector = "Sector",
            Programme = "Programme",
            Level = "Level"
        };

        outerApiMock.Setup(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(getMemberProfileResponse));

        // Act
        var result = sut.GetEditApprenticeshipInformationViewModel(cancellationToken).Result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.EmployerName, Is.EqualTo(employerName));
            Assert.That(result.EmployerAddress1, Is.EqualTo(employerAddress1));
            Assert.That(result.EmployerAddress2, Is.EqualTo(employerAddress2));
            Assert.That(result.EmployerTownOrCity, Is.EqualTo(employerTownOrCity));
            Assert.That(result.EmployerCounty, Is.EqualTo(employerCounty));
            Assert.That(result.EmployerPostcode, Is.EqualTo(employerPostcode));
            Assert.That(result.ShowApprenticeshipInformation, Is.EqualTo(showApprenticeshipInformation));
            Assert.That(result.Sector, Is.EqualTo(getMemberProfileResponse.Apprenticeship!.Sector));
            Assert.That(result.Programmes, Is.EqualTo(getMemberProfileResponse.Apprenticeship!.Programme));
            Assert.That(result.Level, Is.EqualTo(getMemberProfileResponse.Apprenticeship!.Level));
        });
    }

    [Test, RecursiveMoqAutoData]
    public void GetEditApprenticeshipInformationViewModel_ApprenticeshipInformationIsNull_ExpectedValueShouldReturn(
        string employerName,
        string employerAddress1,
        string employerAddress2,
        string employerTownOrCity,
        string employerCounty,
        string employerPostcode,
        bool showApprenticeshipInformation,
        CancellationToken cancellationToken)
    {
        // Arrange
        SetUpOuterApiMock();
        getMemberProfileResponse.Profiles = new List<MemberProfile>()
            {
                new(){ProfileId=ProfileIds.EmployerName,Value=employerName},
                new(){ProfileId=ProfileIds.EmployerAddress1,Value=employerAddress1},
                new(){ProfileId=ProfileIds.EmployerAddress2,Value=employerAddress2},
                new(){ProfileId=ProfileIds.EmployerTownOrCity,Value=employerTownOrCity},
                new(){ProfileId=ProfileIds.EmployerCounty,Value=employerCounty},
                new(){ProfileId=ProfileIds.EmployerPostcode,Value=employerPostcode}
            };
        getMemberProfileResponse.Preferences = new List<MemberPreference>()
            {
                new(){PreferenceId=PreferenceIds.Apprenticeship,Value=showApprenticeshipInformation}
            };
        getMemberProfileResponse.Apprenticeship = null;
        outerApiMock.Setup(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(getMemberProfileResponse));

        // Act
        var result = sut.GetEditApprenticeshipInformationViewModel(cancellationToken).Result;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.EmployerName, Is.EqualTo(employerName));
            Assert.That(result.EmployerAddress1, Is.EqualTo(employerAddress1));
            Assert.That(result.EmployerAddress2, Is.EqualTo(employerAddress2));
            Assert.That(result.EmployerTownOrCity, Is.EqualTo(employerTownOrCity));
            Assert.That(result.EmployerCounty, Is.EqualTo(employerCounty));
            Assert.That(result.EmployerPostcode, Is.EqualTo(employerPostcode));
            Assert.That(result.ShowApprenticeshipInformation, Is.EqualTo(showApprenticeshipInformation));
            Assert.That(result.Sector, Is.EqualTo(null));
            Assert.That(result.Programmes, Is.EqualTo(null));
            Assert.That(result.Level, Is.EqualTo(null));
        });
    }

    [Test]
    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public void GetEditApprenticeshipInformationViewModel_PassValidApprenticeshipInformationPreference_ShouldReturnExpectedPreferenceValue(
        bool showApprenticeshipInformation,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpOuterApiMock();
        getMemberProfileResponse.Preferences = new List<MemberPreference>() {new()
            {
                PreferenceId = PreferenceIds.Apprenticeship,
                Value = showApprenticeshipInformation
            }};
        outerApiMock.Setup(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(getMemberProfileResponse));

        // Act
        var result = sut.GetEditApprenticeshipInformationViewModel(cancellationToken).Result;

        // Assert
        Assert.That(result.ShowApprenticeshipInformation, Is.EqualTo(showApprenticeshipInformation));
    }

    [Test, MoqAutoData]
    public void GetEditApprenticeshipInformationViewModel_EditApprenticeshipInformationViewModel_ShouldHaveExpectedValueForYourAmbassadorProfileUrl(
            CancellationToken cancellationToken
        )
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.GetEditApprenticeshipInformationViewModel(cancellationToken).Result;

        // Assert
        Assert.That(result.YourAmbassadorProfileUrl, Is.EqualTo(YourAmbassadorProfileUrl));
    }

    [Test, MoqAutoData]
    public void Index_EditApprenticeshipInformationViewModel_ShouldHaveNullValueForNetworkHubLink(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditApprenticeshipInformationViewModel;

        // Assert
        Assert.That(viewModel!.NetworkHubLink, Is.Null);
    }

    [TearDown]
    public void TearDown()
    {
        sut?.Dispose();
    }

    private void SetUpControllerWithContext()
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut = new EditApprenticeshipInformationController(outerApiMock.Object, validatorMock.Object, Mock.Of<ISessionService>())
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user.HttpContext!.User } }
        };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
    }

    private void SetUpOuterApiMock()
    {
        outerApiMock = new();
        validatorMock = new();
        SetUpControllerWithContext();

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
