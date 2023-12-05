using AutoFixture.NUnit3;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SFA.DAS.Aan.SharedUi.Infrastructure;
using SFA.DAS.Aan.SharedUi.Models.EditContactDetail;
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
    static readonly string YourAmbassadorProfileUrl = Guid.NewGuid().ToString();
    static Guid memberId = Guid.NewGuid();
    static EditContactDetailViewModel editContactDetailViewModel = new EditContactDetailViewModel();

    [Test, RecursiveMoqAutoData]
    public static async Task Index_PostValidCommand_RedirectToYourAmbassadorProfile(
        string linkedinUrl,
        bool showLinkedinUrl,
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        var validatorMock = new Mock<IValidator<SubmitContactDetailModel>>();
        var successfulValidationResult = new ValidationResult();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<SubmitContactDetailModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(successfulValidationResult);
        EditContactDetailController sut = new EditContactDetailController(outerApiMock.Object, validatorMock.Object);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;
        SubmitContactDetailModel submitContactDetailModel = new()
        {
            LinkedinUrl = linkedinUrl,
            ShowLinkedinUrl = showLinkedinUrl
        };

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
    public static async Task Index_PostValidCommand_TempDataValueIsSet(
        string linkedinUrl,
        bool showLinkedinUrl,
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);

        var validatorMock = new Mock<IValidator<SubmitContactDetailModel>>();
        var successfulValidationResult = new ValidationResult();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<SubmitContactDetailModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(successfulValidationResult);

        EditContactDetailController sut = new EditContactDetailController(outerApiMock.Object, validatorMock.Object);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;
        SubmitContactDetailModel submitContactDetailModel = new()
        {
            LinkedinUrl = linkedinUrl,
            ShowLinkedinUrl = showLinkedinUrl
        };

        // Act
        var response = await sut.Post(submitContactDetailModel, cancellationToken);

        // Assert
        Assert.That(sut.TempData.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage), Is.EqualTo(true));
    }

    [Test, RecursiveMoqAutoData]
    public static async Task Index_PostValidCommand_ShouldInvokeUpdateMemberProfileAndPreferences(
        string linkedinUrl,
        bool showLinkedinUrl,
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        var validatorMock = new Mock<IValidator<SubmitContactDetailModel>>();
        var successfulValidationResult = new ValidationResult();
        validatorMock.Setup(v => v.ValidateAsync(It.IsAny<SubmitContactDetailModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(successfulValidationResult);

        EditContactDetailController sut = new EditContactDetailController(outerApiMock.Object, validatorMock.Object);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;
        SubmitContactDetailModel submitContactDetailModel = new()
        {
            LinkedinUrl = linkedinUrl,
            ShowLinkedinUrl = showLinkedinUrl
        };


        // Act
        var response = await sut.Post(submitContactDetailModel, cancellationToken);

        // Assert
        outerApiMock.Verify(a => a.UpdateMemberProfileAndPreferences(It.IsAny<Guid>(), It.IsAny<UpdateMemberProfileAndPreferencesRequest>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public static async Task Index_PostInValidCommand_ShouldInvokeGetMemberProfile(
        [Frozen] Mock<IOuterApiClient> outerApiMock,
        [Greedy] EditContactDetailController sut,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;
        SubmitContactDetailModel submitContactDetailModel = new()
        {
            LinkedinUrl = string.Empty,
            ShowLinkedinUrl = true
        };

        // Act
        var response = await sut.Post(submitContactDetailModel, cancellationToken);

        // Assert
        outerApiMock.Verify(a => a.GetMemberProfile(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public static async Task Index_PostInValidCommand_ShouldReturnEditContactDetailView(
        [Greedy] EditContactDetailController sut,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;
        SubmitContactDetailModel submitContactDetailModel = new()
        {
            LinkedinUrl = string.Empty,
            ShowLinkedinUrl = true
        };

        // Act
        var result = await sut.Post(submitContactDetailModel, cancellationToken);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult!.ViewName, Does.Contain("EditContactDetail"));
        });
    }

    [Test, RecursiveMoqAutoData]
    public static async Task Index_PostInValidCommand_ShouldReturnEditContactDetailViewModel(
        [Greedy] EditContactDetailController sut,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;
        SubmitContactDetailModel submitContactDetailModel = new()
        {
            LinkedinUrl = string.Empty,
            ShowLinkedinUrl = true
        };

        // Act
        var result = await sut.Post(submitContactDetailModel, cancellationToken);
        var viewResult = result as ViewResult;

        // Assert
        Assert.That(viewResult!.Model, Is.InstanceOf(editContactDetailViewModel.GetType()));
    }

    [Test, RecursiveMoqAutoData]
    public static async Task Index_PostInValidCommand_ShouldHaveExpectedValueForYourAmbassadorProfileUrl(
        [Greedy] EditContactDetailController sut,
        CancellationToken cancellationToken
    )
    {
        // Arrange
        var user = AuthenticatedUsersForTesting.FakeLocalUserFullyVerifiedClaim(memberId);
        sut.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        sut.AddContextWithClaim(ClaimsPrincipalExtensions.ClaimTypes.AanMemberId, Guid.NewGuid().ToString());
        sut.AddUrlHelperMock()
            .AddUrlForRoute(SharedRouteNames.YourAmbassadorProfile, YourAmbassadorProfileUrl);
        Mock<ITempDataDictionary> tempDataMock = new Mock<ITempDataDictionary>();
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.YourAmbassadorProfileSuccessMessage)).Returns(true);
        sut.TempData = tempDataMock.Object;
        SubmitContactDetailModel submitContactDetailModel = new()
        {
            LinkedinUrl = string.Empty,
            ShowLinkedinUrl = true
        };

        // Act
        var result = await sut.Post(submitContactDetailModel, cancellationToken);
        var viewResult = result as ViewResult;
        var viewModel = viewResult!.Model as EditContactDetailViewModel;

        // Assert
        Assert.That(viewModel!.YourAmbassadorProfileUrl, Is.EqualTo(YourAmbassadorProfileUrl));
    }
}
