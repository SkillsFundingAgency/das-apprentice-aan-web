using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Constants;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class EditPersonalInformationControllerGetTests
{
    static readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();

    [Test]
    [MoqAutoData]
    public void Index_ReturnsEditPersonalInformationViewModel(
    [Frozen] Mock<IOuterApiClient> outerApiMock,
    [Greedy] EditPersonalInformationController sut,
    GetMemberProfileResponse memberProfile)
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

        //Act
        var result = (ViewResult)sut.Index(new CancellationToken()).Result;

        //Assert
        Assert.That(result.Model, Is.TypeOf<EditPersonalInformationViewModel>());
    }

    [Test]
    [MoqAutoData]
    public void Index_InvokesOuterApiClientGetMemberProfile(
    [Frozen] Mock<IOuterApiClient> outerApiMock,
    [Greedy] EditPersonalInformationController sut,
    CancellationToken cancellationToken)
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());

        //Act
        var result = sut.Index(cancellationToken);

        //Assert
        outerApiMock.Verify(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    [MoqAutoData]
    public void Index_InvokesOuterApiClientGetRegions(
    [Frozen] Mock<IOuterApiClient> outerApiMock,
    [Greedy] EditPersonalInformationController sut,
    CancellationToken cancellationToken)
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());

        //Act
        var result = sut.Index(cancellationToken);

        //Assert
        outerApiMock.Verify(o => o.GetRegions(), Times.Once());
    }

    [Test]
    [MoqAutoData]
    public void Index_ReturnsProfileView(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        GetMemberProfileResponse getMemberProfileResponse,
        [Greedy] EditPersonalInformationController sut
    )
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        outerApiMock.Setup(o => o.GetMemberProfile(memberId, memberId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(getMemberProfileResponse));
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        sut.AddUrlHelperMock()
    .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);

        //Act
        var result = sut.Index(new CancellationToken());

        //Assert
        Assert.Multiple(async () =>
        {
            var viewResult = await result as ViewResult;
            Assert.That(viewResult!.ViewName, Does.Contain("EditPersonalInformation"));
        });
    }

    [Test]
    [MoqInlineAutoData("test", false, false)]
    [MoqInlineAutoData(null, false, false)]
    [MoqInlineAutoData("test", false, true)]
    [MoqInlineAutoData(null, false, true)]
    [MoqInlineAutoData("test", true, false)]
    [MoqInlineAutoData(null, true, false)]
    [MoqInlineAutoData("test", true, true)]
    [MoqInlineAutoData(null, true, true)]
    public void EditPersonalInformationViewModelMapping_ReturnsEditPersonalInformationViewModel(string? organisationName, bool showJobTitle, bool showBiography, int regionId, MemberUserType userType)
    {
        //Arrange
        var fixture = new Fixture();
        EditPersonalInformationViewModel editPersonalInformationViewModel = new EditPersonalInformationViewModel();
        IEnumerable<MemberProfile> memberProfiles = fixture.CreateMany<MemberProfile>(4);
        IEnumerable<MemberPreference> memberPreferences = fixture.CreateMany<MemberPreference>();
        memberProfiles.ToArray()[0].ProfileId = ProfileConstants.ProfileIds.JobTitle;
        memberPreferences.ToArray()[0].PreferenceId = PreferenceConstants.PreferenceIds.JobTitle;
        memberProfiles.ToArray()[0].PreferenceId = memberPreferences.ToArray()[0].PreferenceId;
        memberPreferences.ToArray()[0].Value = showJobTitle;
        memberProfiles.ToArray()[1].ProfileId = ProfileConstants.ProfileIds.Biography;
        memberPreferences.ToArray()[1].PreferenceId = PreferenceConstants.PreferenceIds.Biography;
        memberProfiles.ToArray()[1].PreferenceId = memberPreferences.ToArray()[1].PreferenceId;
        memberPreferences.ToArray()[1].Value = showBiography;

        //Act
        var sut = EditPersonalInformationController.EditPersonalInformationViewModelMapping(regionId, memberProfiles, memberPreferences, userType, organisationName);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut, Is.InstanceOf(editPersonalInformationViewModel.GetType()));
            Assert.That(sut.RegionId, Is.EqualTo(regionId));
            Assert.That(sut.UserType, Is.EqualTo(userType));
            Assert.That(sut.OrganisationName, Is.EqualTo(organisationName ?? string.Empty));
            Assert.That(sut.ShowJobTitle, Is.EqualTo(showJobTitle));
            Assert.That(sut.ShowBiography, Is.EqualTo(showBiography));
            Assert.That(sut.JobTitle, Is.EqualTo(memberProfiles.ToArray()[0].Value));
            Assert.That(sut.Biography, Is.EqualTo(memberProfiles.ToArray()[1].Value));
        });
    }
}