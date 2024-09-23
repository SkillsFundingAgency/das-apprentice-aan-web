using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.EditContactDetail;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.Aan.SharedUi.Constants.PreferenceConstants;
using static SFA.DAS.Aan.SharedUi.Constants.ProfileConstants;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.EditContactDetailControllerTests;
public class EditContactDetailControllerGetTests
{
    private EditContactDetailController sut = null!;
    private Mock<IOuterApiClient> outerApiMock = null!;
    private Mock<IValidator<SubmitContactDetailModel>> validatorMock = null!;
    private readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();
    private readonly Guid memberId = Guid.NewGuid();
    private GetMemberProfileResponse getMemberProfileResponse = null!;

    private void SetUpControllerWithContext()
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut = new EditContactDetailController(outerApiMock.Object, validatorMock.Object, Mock.Of<ISessionService>())
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

    [Test, RecursiveMoqAutoData]
    public void Index_ShouldReturnEditContactDetailView(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.Index(cancellationToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.ViewName, Does.Contain(SharedRouteNames.EditContactDetail));
        });
    }

    [Test, RecursiveMoqAutoData]
    public void Index_ShouldInvokeGetMemberProfile(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.Index(cancellationToken);

        // Assert
        outerApiMock.Verify(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), false, cancellationToken), Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public void Index_PassValidEmailAndLinkedinUrl_ShouldReturnExpectedEmailAndLinkedinUrl(
        [Frozen] Mock<IOuterApiClient> outerApiClient,
        string email,
        string linkedinUrl,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpOuterApiMock();
        getMemberProfileResponse.Email = email;
        getMemberProfileResponse.Profiles = new List<MemberProfile>() {new()
        {
            ProfileId = ProfileIds.LinkedIn,
            Value = linkedinUrl
        }};
        outerApiClient.Setup(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(getMemberProfileResponse));

        // Act
        var result = sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditContactDetailViewModel;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(viewModel!.Email, Is.EqualTo(email));
            Assert.That(viewModel!.LinkedinUrl, Is.EqualTo(linkedinUrl));
        });
    }

    [Test]
    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public void Index_PassValidLinkedinUrlPreference_ShouldReturnExpectedPreferenceValue(
        bool showLinkedinUrl,
        GetMemberProfileResponse getMemberProfileResponse,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        outerApiMock = new();
        validatorMock = new();
        SetUpControllerWithContext();
        getMemberProfileResponse.Preferences = new List<MemberPreference>() {new()
        {
            PreferenceId = PreferenceIds.LinkedIn,
            Value = showLinkedinUrl
        }};
        outerApiMock.Setup(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(getMemberProfileResponse));

        // Act
        var result = sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditContactDetailViewModel;

        // Assert
        Assert.That(viewModel!.ShowLinkedinUrl, Is.EqualTo(showLinkedinUrl));
    }

    [Test, MoqAutoData]
    public void Index_EditContactDetailViewModel_ShouldHaveExpectedValueForYourAmbassadorProfileUrl(
         CancellationToken cancellationToken
       )
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditContactDetailViewModel;

        // Assert
        Assert.That(viewModel!.YourAmbassadorProfileUrl, Is.EqualTo(YourAmbassadorProfileUrl));
    }

    [Test, MoqAutoData]
    public async Task GetContactDetailViewModel_ShouldReturnEditContactDetailViewModel(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = await sut.GetContactDetailViewModel(cancellationToken);

        // Assert
        result.Should().BeOfType<EditContactDetailViewModel>();
    }

    [Test, MoqAutoData]
    public async Task GetContactDetailViewModel_ShouldInvokeGetMemberProfile(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = await sut.GetContactDetailViewModel(cancellationToken);

        // Assert
        outerApiMock.Verify(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, MoqAutoData]
    public void Index_EditContactDetailViewModel_ShouldHaveNullValueForNetworkHubLink(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpOuterApiMock();

        // Act
        var result = sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditContactDetailViewModel;

        // Assert
        Assert.That(viewModel!.NetworkHubLink, Is.Null);
    }

    [TearDown]
    public void TearDown()
    {
        sut?.Dispose();
    }
}
