using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.EditContactDetail;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
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
    static readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();
    static EditContactDetailViewModel editContactDetailViewModel = new EditContactDetailViewModel();
    static Guid memberId = Guid.NewGuid();

    [Test, RecursiveMoqAutoData]
    public static async Task Index_ShouldReturnEditContactDetailView(
        [Greedy] EditContactDetailController sut,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
.AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        // Act
        var result = await sut.Index(cancellationToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.ViewName, Does.Contain("EditContactDetail"));
        });
    }

    [Test, RecursiveMoqAutoData]
    public void Index_ShouldInvokeGetMemberProfile(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] EditContactDetailController sut,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
.AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        // Act
        var result = sut.Index(cancellationToken);

        // Assert
        outerApiMock.Verify(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), false, cancellationToken), Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public static async Task Index_ShouldReturnEditContactDetailViewModel(
        [Greedy] EditContactDetailController sut,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
.AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        // Act
        var result = await sut.Index(cancellationToken);
        var viewResult = result as ViewResult;

        // Assert
        Assert.That(viewResult!.Model, Is.InstanceOf(editContactDetailViewModel.GetType()));
    }

    [Test, RecursiveMoqAutoData]
    public static async Task Index_PassValidEmailAndLinkedinUrl_ShouldReturnExpectedEmailAndLinkedinUrl(
        [Frozen] Mock<IOuterApiClient> outerApiClient,
        [Greedy] EditContactDetailController sut,
        GetMemberProfileResponse getMemberProfileResponse,
        string email,
        string linkedinUrl,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
.AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        getMemberProfileResponse.Email = email;
        getMemberProfileResponse.Profiles = new List<MemberProfile>() {new()
        {
            ProfileId = ProfileIds.LinkedIn,
            Value = linkedinUrl
        }};
        outerApiClient.Setup(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(getMemberProfileResponse));

        // Act
        var result = await sut.Index(cancellationToken);
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
    public static async Task Index_PassValidLinkedinUrlPreference_ShouldReturnExpectedPreferenceValue(
        bool showLinkedinUrl,
        [Frozen] Mock<IOuterApiClient> outerApiClient,
        [Greedy] EditContactDetailController sut,
        GetMemberProfileResponse getMemberProfileResponse,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
.AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        getMemberProfileResponse.Preferences = new List<MemberPreference>() {new()
        {
            PreferenceId = PreferenceIds.LinkedIn,
            Value = showLinkedinUrl
        }};
        outerApiClient.Setup(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(getMemberProfileResponse));

        // Act
        var result = await sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditContactDetailViewModel;

        // Assert
        Assert.That(viewModel!.ShowLinkedinUrl, Is.EqualTo(showLinkedinUrl));
    }

    [Test, MoqAutoData]
    public static async Task Index_EditContactDetailViewModel_ShouldHaveExpectedValueForYourAmbassadorProfileUrl(
            [Greedy] EditContactDetailController sut,
            CancellationToken cancellationToken
        )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
    .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        // Act
        var result = await sut.Index(cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditContactDetailViewModel;

        // Assert
        Assert.That(viewModel!.YourAmbassadorProfileUrl, Is.EqualTo(YourAmbassadorProfileUrl));
    }

    [Test, MoqAutoData]
    public static async Task GetContactDetailViewModel_ShouldReturnEditContactDetailViewModel(
        [Greedy] EditContactDetailController sut,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
    .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        // Act
        var result = await sut.GetContactDetailViewModel(cancellationToken);

        // Assert
        Assert.That(result, Is.InstanceOf(editContactDetailViewModel.GetType()));
    }

    [Test, MoqAutoData]
    public static async Task GetContactDetailViewModel_ShouldInvokeGetMemberProfile(
        [Frozen] Mock<IOuterApiClient> outerApiClient,
        [Greedy] EditContactDetailController sut,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
    .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        // Act
        var result = await sut.GetContactDetailViewModel(cancellationToken);

        // Assert
        outerApiClient.Verify(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
