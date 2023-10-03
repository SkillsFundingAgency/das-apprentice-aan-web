using System.Net;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestEase;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
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
        MemberProfile memberProfile)
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        var response = new Response<MemberProfile>(string.Empty, new(HttpStatusCode.OK), () => memberProfile);
        outerApiMock.Setup(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);

        //Act
        var result = (ViewResult)sut.Get(memberId, true, new CancellationToken()).Result;

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
        var result = sut.Get(memberId, true, cancellationToken);

        //Assert
        outerApiMock.Verify(o => o.GetMemberProfile(memberId, It.IsAny<Guid>(), true, cancellationToken), Times.Once());
    }

    [Test]
    [MoqAutoData]
    public void Details_MemberProfileIdIsNotFound_ThrowsInvalidOperationException(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] MemberProfileController sut,
        CancellationToken cancellationToken)
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        var response = new Response<MemberProfile>(string.Empty, new(HttpStatusCode.NotFound), () => null!);
        outerApiMock.Setup(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());

        //Assert
        Assert.That(() => sut.Get(memberId, true, cancellationToken), Throws.InvalidOperationException);
    }

    [Test]
    [MoqAutoData]
    public void Get_ReturnsProfileView(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        CancellationToken cancellationToken,
        MemberProfile memberProfile
    )
    {
        //Arrange
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        var response = new Response<MemberProfile>(string.Empty, new(HttpStatusCode.OK), () => memberProfile);
        outerApiMock.Setup(o => o.GetMemberProfile(memberId, memberId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(response);
        MemberProfileController sut = new MemberProfileController(outerApiMock.Object);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());

        //Act
        var result = sut.Get(memberId, true, cancellationToken);

        //Assert
        Assert.Multiple(async () =>
        {
            var viewResult = await result as ViewResult;
            Assert.That(viewResult!.ViewName!.Contains("Profile"));
        });
    }
}
