using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.AmbassadorProfile;
using SFA.DAS.Aan.SharedUi.Models.EditContactDetail;
using SFA.DAS.Aan.SharedUi.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Web.Controllers;
using SFA.DAS.ApprenticeAan.Web.Extensions;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.ApprenticePortal.Authentication.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.EditContactDetailControllerTests;
public class EditContactDetailControllerPostTests
{
    private EditContactDetailController sut = null!;
    private Mock<IOuterApiClient> outerApiMock = null!;
    private Mock<IValidator<SubmitContactDetailModel>> validatorMock = null!;
    private readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();
    private readonly Guid memberId = Guid.NewGuid();
    private SubmitContactDetailModel submitContactDetailModel = null!;
    private GetMemberProfileResponse getMemberProfileResponse = null!;

    [TearDown]
    public void TearDown()
    {
        sut?.Dispose();
    }
    private void SetUpControllerWithContext()
    {
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut = new EditContactDetailController(outerApiMock.Object, validatorMock.Object, Mock.Of<ISessionService>())
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
        };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
    }

    private void SetUpModelValidateTrue()
    {
        outerApiMock = new();
        validatorMock = new();
        SetUpControllerWithContext();

        submitContactDetailModel = new()
        {
            LinkedinUrl = "LinkedinUrl",
            ShowLinkedinUrl = true
        };

        Mock<ITempDataDictionary> tempDataMock = new();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<SubmitContactDetailModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
    }

    private void SetUpModelValidateFalse()
    {
        outerApiMock = new();
        validatorMock = new();
        SetUpControllerWithContext();

        submitContactDetailModel = new()
        {
            LinkedinUrl = string.Empty,
            ShowLinkedinUrl = true
        };

        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<SubmitContactDetailModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult(new List<ValidationFailure>()
            {
                new("TestField","Test Message"){ErrorCode = "1001"}
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

    [Test, RecursiveMoqAutoData]
    public async Task Post_PostValidCommand_RedirectToYourAmbassadorProfile(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpModelValidateTrue();

        // Act
        var response = await sut.Post(submitContactDetailModel, cancellationToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.TypeOf<RedirectToRouteResult>());
            var redirectToAction = (RedirectToRouteResult)response;
            Assert.That(redirectToAction.RouteName, Does.Contain(SharedRouteNames.YourAmbassadorProfile));
        });
    }

    [Test, RecursiveMoqAutoData]
    public async Task Post_PostValidCommand_TempDataValueIsSet(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpModelValidateTrue();

        // Act
        await sut.Post(submitContactDetailModel, cancellationToken);

        // Assert
        Assert.That(sut.TempData.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage), Is.EqualTo(true));
    }

    [Test, RecursiveMoqAutoData]
    public async Task Post_PostValidCommand_ShouldInvokeUpdateMemberProfileAndPreferences(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpModelValidateTrue();

        // Act
        var response = await sut.Post(submitContactDetailModel, cancellationToken);

        // Assert
        outerApiMock.Verify(a => a.UpdateMemberProfileAndPreferences(It.IsAny<Guid>(), It.IsAny<UpdateMemberProfileAndPreferencesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [TestCase("linkedinurl  ", "linkedinurl")]
    [TestCase("linkedinurl", "linkedinurl")]
    [TestCase("", "")]
    [TestCase(null, null)]
    public async Task Post_SetsLinkedinUrl(string? linkedinUrl, string? expectedLinkedinUrl)
    {
        // Arrange
        SetUpModelValidateTrue();
        submitContactDetailModel.LinkedinUrl = linkedinUrl;

        //Act
        await sut.Post(submitContactDetailModel, CancellationToken.None);

        //Assert
        outerApiMock.Verify(a => a.UpdateMemberProfileAndPreferences(
            It.IsAny<Guid>(),
            It.Is<UpdateMemberProfileAndPreferencesRequest>(m => m.UpdateMemberProfileRequest.MemberProfiles.ElementAt(0).Value == expectedLinkedinUrl),
            It.IsAny<CancellationToken>()));
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Post_SetsShowLinkedinUrl(bool showLinkedinUrl)
    {
        // Arrange
        SetUpModelValidateTrue();
        submitContactDetailModel.ShowLinkedinUrl = showLinkedinUrl;

        //Act
        await sut.Post(submitContactDetailModel, CancellationToken.None);

        //Assert
        outerApiMock.Verify(a => a.UpdateMemberProfileAndPreferences(
            It.IsAny<Guid>(),
            It.Is<UpdateMemberProfileAndPreferencesRequest>(m => m.UpdateMemberProfileRequest.MemberPreferences.ElementAt(0).Value == showLinkedinUrl),
            It.IsAny<CancellationToken>()));
    }

    [Test, RecursiveMoqAutoData]
    public async Task Post_PostInValidCommand_ShouldInvokeGetMemberProfile(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpModelValidateFalse();

        // Act
        await sut.Post(submitContactDetailModel, cancellationToken);

        // Assert
        outerApiMock.Verify(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Post_PostInValidCommand_ShouldReturnEditContactDetailView(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpModelValidateFalse();

        // Act
        var result = await sut.Post(submitContactDetailModel, cancellationToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.ViewName, Does.Contain(SharedRouteNames.EditContactDetail));
        });
    }

    [Test, RecursiveMoqAutoData]
    public async Task Post_PostInValidCommand_ShouldReturnEditContactDetailViewModel(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpModelValidateFalse();

        // Act
        var result = await sut.Post(submitContactDetailModel, cancellationToken);
        var viewResult = result as ViewResult;

        // Assert
        viewResult!.Model.Should().BeOfType<EditContactDetailViewModel>();
    }

    [Test, RecursiveMoqAutoData]
    public async Task Post_PostInValidCommand_ShouldHaveExpectedValueForYourAmbassadorProfileUrl(
        CancellationToken cancellationToken
    )
    {
        // Arrange
        SetUpModelValidateFalse();

        // Act
        var result = await sut.Post(submitContactDetailModel, cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditContactDetailViewModel;

        // Assert
        Assert.That(viewModel!.YourAmbassadorProfileUrl, Is.EqualTo(YourAmbassadorProfileUrl));
    }
}
