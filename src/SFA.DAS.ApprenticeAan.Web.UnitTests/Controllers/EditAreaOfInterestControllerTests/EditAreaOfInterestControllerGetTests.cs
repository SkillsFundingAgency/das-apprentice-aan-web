using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.EditAreaOfInterest;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class EditAreaOfInterestControllerGetTests
{
    static readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();

    [Test, MoqAutoData]
    public void Get_ReturnsEditAreaOfInterestViewModel(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] EditAreaOfInterestController sut,
        GetMemberProfileResponse memberProfile,
        GetProfilesResult getProfilesResult)
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        outerApiMock.Setup(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(memberProfile));
        outerApiMock.Setup(o => o.GetProfilesByUserType(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(getProfilesResult));

        //Act
        var result = (ViewResult)sut.Get(new CancellationToken());

        //Assert
        Assert.That(result.Model, Is.TypeOf<EditAreaOfInterestViewModel>());
    }

    [Test, MoqAutoData]
    public void Index_InvokesOuterApiClientGetMemberProfile(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] EditAreaOfInterestController sut,
        CancellationToken cancellationToken)
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
.AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        //Act
        var result = sut.GetAreaOfInterests(cancellationToken);

        //Assert
        outerApiMock.Verify(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test, MoqAutoData]
    public void Index_InvokesOuterApiClientGetProfilesByUserType(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] EditAreaOfInterestController sut,
        CancellationToken cancellationToken)
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
.AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        //Act
        var result = sut.GetAreaOfInterests(cancellationToken);

        //Assert
        outerApiMock.Verify(o => o.GetProfilesByUserType(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test, MoqAutoData]
    public void Index_ReturnsEditAreaOfInterestView(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        GetMemberProfileResponse getMemberProfileResponse,
        GetProfilesResult getProfilesResult,
        [Greedy] EditAreaOfInterestController sut
    )
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        outerApiMock.Setup(o => o.GetMemberProfile(memberId, memberId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(getMemberProfileResponse));
        outerApiMock.Setup(o => o.GetProfilesByUserType(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(getProfilesResult));
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
    .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        //Act
        var result = sut.Get(new CancellationToken());

        //Assert
        Assert.Multiple(() =>
        {
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.ViewName, Does.Contain("EditAreaOfInterest"));
        });
    }

    [Test]
    [MoqInlineAutoData(true)]
    [MoqInlineAutoData(false)]
    public void SelectProfileViewModelMapping_ReturnsSelectProfileViewModelList(bool profileValue)
    {
        //Arrange
        var fixture = new Fixture();
        EditPersonalInformationViewModel editPersonalInformationViewModel = new EditPersonalInformationViewModel();
        IEnumerable<MemberProfile> memberProfiles = fixture.CreateMany<MemberProfile>(1);

        List<SelectProfileViewModel> selectProfileViewModels = new List<SelectProfileViewModel>();

        memberProfiles.FirstOrDefault()!.Value = profileValue.ToString();
        List<Profile> profiles = new List<Profile>()
        {
            new Profile{ Id=memberProfiles.FirstOrDefault()!.ProfileId}
        };

        //Act
        var sut = EditAreaOfInterestController.SelectProfileViewModelMapping(profiles, memberProfiles);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut, Is.InstanceOf(selectProfileViewModels.GetType()));
            Assert.That(sut, Has.Count.EqualTo(1));
            Assert.That(sut[0].Id, Is.EqualTo(profiles.ToArray()[0].Id));
            Assert.That(sut[0].Description, Is.EqualTo(profiles.ToArray()[0].Description));
            Assert.That(sut[0].Category, Is.EqualTo(profiles.ToArray()[0].Category));
            Assert.That(sut[0].Ordering, Is.EqualTo(profiles.ToArray()[0].Ordering));
            Assert.That(sut[0].IsSelected, Is.EqualTo(profileValue));
        });
    }

    [Test, MoqAutoData]
    public void Index_EditAreaOfInterestViewModel_ShouldHaveNullValueForNetworkHubLink(
    [Frozen] Mock<IOuterApiClient> outerApiMock,
    GetMemberProfileResponse getMemberProfileResponse,
    GetProfilesResult getProfilesResult,
    [Greedy] EditAreaOfInterestController sut
 )
    {
        // Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        outerApiMock.Setup(o => o.GetMemberProfile(memberId, memberId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(getMemberProfileResponse));
        outerApiMock.Setup(o => o.GetProfilesByUserType(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(getProfilesResult));
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
    .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        // Act
        var result = sut.Get(new CancellationToken());
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditAreaOfInterestViewModel;

        // Assert
        Assert.That(viewModel!.NetworkHubLink, Is.Null);
    }
}