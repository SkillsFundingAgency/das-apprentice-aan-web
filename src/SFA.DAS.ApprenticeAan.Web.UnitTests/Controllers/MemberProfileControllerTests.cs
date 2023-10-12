using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers;

public class MemberProfileControllerTests
{
    [Test]
    [MoqAutoData]
    public void MemberProfile_ReturnsMemberProfileViewModel(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] MemberProfileController sut,
        GetMemberProfileResponse memberProfile)
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        outerApiMock.Setup(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(memberProfile));

        //Act
        var result = (ViewResult)sut.Get(memberId, new CancellationToken()).Result;

        //Assert
        Assert.That(result.Model, Is.TypeOf<MemberProfileViewModel>());
    }

    [Test]
    [MoqAutoData]
    public void Details_InvokesOuterApiClientGetMemberProfile(
    [Frozen] Mock<IOuterApiClient> outerApiMock,
    [Greedy] MemberProfileController sut,
    CancellationToken cancellationToken)
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());

        //Act
        var result = sut.Get(memberId, cancellationToken);

        //Assert
        outerApiMock.Verify(o => o.GetMemberProfile(memberId, It.IsAny<Guid>(), true, cancellationToken), Times.Once());
    }

    [Test]
    [MoqAutoData]
    public void Get_ReturnsProfileView(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        CancellationToken cancellationToken,
        GetMemberProfileResponse getMemberProfileResponse
    )
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        outerApiMock.Setup(o => o.GetMemberProfile(memberId, memberId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(getMemberProfileResponse));
        MemberProfileController sut = new MemberProfileController(outerApiMock.Object);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());

        //Act
        var result = sut.Get(memberId, cancellationToken);

        //Assert
        Assert.Multiple(async () =>
        {
            var viewResult = await result as ViewResult;
            Assert.That(viewResult!.ViewName, Does.Contain("Profile"));
        });
    }

    [Test]
    [MoqAutoData]
    public void MemberProfileDetailMapping_ReturnsMemberProfileDetail(Task<GetMemberProfileResponse> memberProfiles)
    {
        //Arrange
        MemberProfileDetail memberProfileDetail = new MemberProfileDetail();

        //Act
        var sut = MemberProfileController.MemberProfileDetailMapping(memberProfiles);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut, Is.InstanceOf(memberProfileDetail.GetType()));
            Assert.That(sut.FullName, Is.EqualTo(memberProfiles.Result.FullName));
            Assert.That(sut.FirstName, Is.EqualTo(memberProfiles.Result.FirstName));
            Assert.That(sut.LastName, Is.EqualTo(memberProfiles.Result.LastName));
            Assert.That(sut.Email, Is.EqualTo(memberProfiles.Result.Email));
            Assert.That(sut.RegionId, Is.EqualTo(memberProfiles.Result.RegionId));
            Assert.That(sut.OrganisationName, Is.EqualTo(memberProfiles.Result.OrganisationName));
            Assert.That(sut.UserType, Is.EqualTo(memberProfiles.Result.UserType));
            Assert.That(sut.IsRegionalChair, Is.EqualTo(memberProfiles.Result.IsRegionalChair));
            Assert.That(sut.Profiles, Is.EqualTo(memberProfiles.Result.Profiles));
            if (memberProfiles.Result.Apprenticeship != null)
            {
                Assert.That(sut.Sector, Is.EqualTo(memberProfiles.Result.Apprenticeship!.Sector));
                Assert.That(sut.Programmes, Is.EqualTo(memberProfiles.Result.Apprenticeship!.Programme));
                Assert.That(sut.Level, Is.EqualTo(memberProfiles.Result.Apprenticeship!.Level));
            }
        });
    }
}