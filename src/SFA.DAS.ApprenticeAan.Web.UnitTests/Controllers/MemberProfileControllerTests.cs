using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.PublicProfile;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
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
    [MoqInlineAutoData(MemberUserType.Apprentice)]
    [MoqInlineAutoData(MemberUserType.Employer)]
    public void MemberProfile_ReturnsMemberProfileViewModel(
        MemberUserType memberUserType,
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] MemberProfileController sut,
        GetMemberProfileResponse memberProfile)
    {
        //Arrange
        memberProfile.Preferences = Enumerable.Range(1, 4).Select(id => new MemberPreference { PreferenceId = id, Value = true });
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

        memberProfile.UserType = memberUserType;
        outerApiMock.Setup(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(memberProfile);
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.MemberId)).Returns(memberId.ToString());
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
    GetMemberProfileResponse getMemberProfileResponse,
    CancellationToken cancellationToken)
    {
        //Arrange
        getMemberProfileResponse.Preferences = Enumerable.Range(1, 4).Select(id => new MemberPreference { PreferenceId = id, Value = true });
        outerApiMock.Setup(o => o.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(getMemberProfileResponse);
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
    [MoqInlineAutoData(MemberUserType.Apprentice)]
    [MoqInlineAutoData(MemberUserType.Employer)]
    public void Get_ReturnsProfileView(
        MemberUserType memberUserType,
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Frozen] Mock<ISessionService> sessionServiceMock,
        GetMemberProfileResponse getMemberProfileResponse,
        Mock<IValidator<ConnectWithMemberSubmitModel>> validatorMock,
        CancellationToken cancellationToken
    )
    {
        //Arrange
        getMemberProfileResponse.Preferences = Enumerable.Range(1, 4).Select(id => new MemberPreference { PreferenceId = id, Value = true });
        var memberId = Guid.NewGuid();
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        getMemberProfileResponse.UserType = memberUserType;
        outerApiMock.Setup(o => o.GetMemberProfile(memberId, memberId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(getMemberProfileResponse));
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.MemberId)).Returns(memberId.ToString());
        MemberProfileController sut = new MemberProfileController(outerApiMock.Object, validatorMock.Object, sessionServiceMock.Object);
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
    [MoqInlineAutoData(MemberUserType.Apprentice)]
    [MoqInlineAutoData(MemberUserType.Employer)]
    public async Task Post_InvalidCommand_ReturnsMemberProfileView(
        MemberUserType userType,
        ConnectWithMemberSubmitModel command,
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Frozen] Mock<ISessionService> sessionServiceMock,
        GetMemberProfileResponse getMemberProfileResponse,
        Mock<IValidator<ConnectWithMemberSubmitModel>> validatorMock,
        CancellationToken cancellationToken)
    {
        //Arrange
        getMemberProfileResponse.Preferences = Enumerable.Range(1, 4).Select(id => new MemberPreference { PreferenceId = id, Value = true });
        command.ReasonToGetInTouch = 0;
        var memberId = Guid.NewGuid();
        getMemberProfileResponse.UserType = userType;
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        outerApiMock.Setup(o => o.GetMemberProfile(memberId, memberId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(getMemberProfileResponse));
        sessionServiceMock.Setup(x => x.Get(Constants.SessionKeys.Member.MemberId)).Returns(memberId.ToString());
        MemberProfileController sut = new MemberProfileController(outerApiMock.Object, validatorMock.Object, sessionServiceMock.Object);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());

        //Act
        var response = await sut.Post(memberId, command, cancellationToken);

        //Assert
        Assert.That(response, Is.InstanceOf<ViewResult>());
    }

    [Test]
    [MoqInlineAutoData(MemberUserType.Apprentice)]
    [MoqInlineAutoData(MemberUserType.Employer)]
    public async Task Post_ValidCommand_ReturnsMemberProfileView(
        MemberUserType userType,
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        GetMemberProfileResponse getMemberProfileResponse,
        CreateNotificationResponse createNotificationResponse,
        CancellationToken cancellationToken)
    {
        //Arrange
        getMemberProfileResponse.Preferences = Enumerable.Range(1, 4).Select(id => new MemberPreference { PreferenceId = id, Value = true });
        ConnectWithMemberSubmitModel command = new()
        {
            ReasonToGetInTouch = 2,
            HasAgreedToCodeOfConduct = true,
            HasAgreedToSharePersonalDetails = true
        };
        var memberId = Guid.NewGuid();
        getMemberProfileResponse.UserType = userType;
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        outerApiMock.Setup(o => o.GetMemberProfile(memberId, memberId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(getMemberProfileResponse));
        var validatorMock = new Mock<IValidator<ConnectWithMemberSubmitModel>>();
        var successfulValidationResult = new ValidationResult();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ConnectWithMemberSubmitModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(successfulValidationResult);

        MemberProfileController sut = new MemberProfileController(outerApiMock.Object, validatorMock.Object, Mock.Of<ISessionService>());

        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, memberId.ToString());
        outerApiMock.Setup(c => c.PostNotification(It.IsAny<Guid>(), It.IsAny<CreateNotificationRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(createNotificationResponse);

        //Act
        var response = await sut.Post(memberId, command, cancellationToken);

        //Assert
        using (new AssertionScope())
        {
            response.Should().BeOfType<RedirectToRouteResult>();
            response.As<RedirectToRouteResult>().RouteName.Should().Be(SharedRouteNames.NotificationSentConfirmation);
        };
    }

    [Test]
    [MoqAutoData]
    public void NotificationSentConfirmation_Returns_View([Frozen] Mock<IOuterApiClient> outerApiMock)
    {
        // Arrange
        var validatorMock = new Mock<IValidator<ConnectWithMemberSubmitModel>>();
        MemberProfileController sut = new MemberProfileController(outerApiMock.Object, validatorMock.Object, Mock.Of<ISessionService>());
        string NetworkDirectoryUrl = Guid.NewGuid().ToString();
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.NetworkDirectory, NetworkDirectoryUrl);

        // Act
        IActionResult result = sut.NotificationSentConfirmation();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());
    }
}
