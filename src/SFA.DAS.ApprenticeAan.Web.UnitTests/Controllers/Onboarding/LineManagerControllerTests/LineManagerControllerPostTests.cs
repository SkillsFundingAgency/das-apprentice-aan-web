using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using RestEase;
using SFA.DAS.Aan.SharedUi.Models;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Requests;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.ApprenticeAan.Web.Extensions.ClaimsPrincipalExtensions;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.LineManagerControllerTests;

[TestFixture]
public class LineManagerControllerPostTests
{
    [Test, MoqAutoData]
    public async Task Post_HasNotSeenTermsAndConditions_RedirectToTheStartOfTheJourney(
        [Greedy] LineManagerController sut,
        Mock<ITempDataDictionary> tempDataMock,
        LineManagerSubmitModel submitModel,
        CancellationToken cancellationToken)
    {
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(false);
        sut.TempData = tempDataMock.Object;

        var result = await sut.Post(submitModel, cancellationToken);

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.BeforeYouStart);
    }

    [Test, MoqAutoData]
    public async Task Post_ModelStateIsInvalid_ReloadsViewWithValidationErrors(
        [Greedy] LineManagerController sut,
        Mock<ITempDataDictionary> tempDataMock,
        string termsAndConditionsUrl,
        CancellationToken cancellationToken)
    {
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);
        sut.TempData = tempDataMock.Object;
        sut.ModelState.AddModelError("key", "message");
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.TermsAndConditions, termsAndConditionsUrl);
        LineManagerSubmitModel submitModel = new() { HasEmployersApproval = null };

        var result = await sut.Post(submitModel, cancellationToken);

        result.As<ViewResult>().ViewName.Should().Be(LineManagerController.ViewPath);
        result.As<ViewResult>().Model.As<LineManagerViewModel>().BackLink.Should().Be(termsAndConditionsUrl);
    }

    [Test, MoqAutoData]
    public async Task Post_DoesNotHaveLineManagersApproval_RedirectsToTheShutterPage(
        [Frozen] ApplicationConfiguration applicationConfiguration,
        [Frozen] Mock<IValidator<LineManagerSubmitModel>> validatorMock,
        [Greedy] LineManagerController sut,
        Mock<ITempDataDictionary> tempDataMock,
        CancellationToken cancellationToken)
    {
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);
        sut.TempData = tempDataMock.Object;
        sut.AddUrlHelperMock();
        LineManagerSubmitModel submitModel = new() { HasEmployersApproval = false };
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());

        var result = await sut.Post(submitModel, cancellationToken);

        result.As<ViewResult>().ViewName.Should().Be(LineManagerController.ShutterPageViewPath);
        result.As<ViewResult>().Model.As<ShutterPageViewModel>().ApprenticeHomeUrl.Should().Be(applicationConfiguration.ApplicationUrls.ApprenticeHomeUrl.ToString());
    }

    [Test, MoqAutoData]
    public async Task Post_DoesNotHaveLineManagersApproval_ClearsSession(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<LineManagerSubmitModel>> validatorMock,
        [Greedy] LineManagerController sut,
        Mock<ITempDataDictionary> tempDataMock,
        CancellationToken cancellationToken)
    {
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);
        sut.TempData = tempDataMock.Object;
        LineManagerSubmitModel submitModel = new() { HasEmployersApproval = false };
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());

        await sut.Post(submitModel, cancellationToken);

        sessionServiceMock.Verify(s => s.Delete<OnboardingSessionModel>());
        tempDataMock.Verify(t => t.Remove(TempDataKeys.HasSeenTermsAndConditions));
    }

    [Test, MoqAutoData]
    public async Task Post_HasSeenTermsAndHasLineManagerApprovalAndModelNotInitialised_InitializesSessionModel(
        [Frozen] Mock<IValidator<LineManagerSubmitModel>> validatorMock,
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IOuterApiClient> apiClient,
        [Frozen] Mock<ITempDataDictionary> tempDataMock,
        [Greedy] LineManagerController sut,
        List<Profile> profiles,
        Guid apprenticeId,
        MyApprenticeship myApprenticeship,
        ApprenticeAccount apprenticeAccount,
        CancellationToken cancellationToken)
    {
        sut.AddContextWithClaim(ClaimTypes.ApprenticeId, apprenticeId.ToString());
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);
        sut.TempData = tempDataMock.Object;
        LineManagerSubmitModel submitModel = new() { HasEmployersApproval = true };
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());
        apiClient.Setup(p => p.GetProfilesByUserType("apprentice", cancellationToken)).ReturnsAsync(new GetProfilesResult { Profiles = profiles });
        apiClient.Setup(c => c.TryCreateMyApprenticeship(It.IsAny<TryCreateMyApprenticeshipRequest>(), cancellationToken)).ReturnsAsync(new Response<MyApprenticeship?>(string.Empty, new(HttpStatusCode.OK), () => myApprenticeship));
        apiClient.Setup(c => c.GetApprenticeAccount(apprenticeId, cancellationToken)).ReturnsAsync(new Response<ApprenticeAccount?>(string.Empty, new(HttpStatusCode.OK), () => apprenticeAccount));

        await sut.Post(submitModel, cancellationToken);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m =>
            m.HasAcceptedTerms
            && m.ProfileData.Count == profiles.Count
            && m.MyApprenticeship == myApprenticeship
            && m.ApprenticeDetails.ApprenticeId == apprenticeId
            && m.ApprenticeDetails.Email == apprenticeAccount.Email
            && m.ApprenticeDetails.Name.StartsWith(apprenticeAccount.FirstName)
            && m.ApprenticeDetails.Name.EndsWith(apprenticeAccount.LastName))));
    }

    [Test, MoqAutoData]
    public async Task Post_MyApprenticeshipNotFoundDuringModelInitialisation_RedirectsToAccessDenied(
        [Frozen] Mock<IValidator<LineManagerSubmitModel>> validatorMock,
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IOuterApiClient> apiClient,
        [Frozen] Mock<ITempDataDictionary> tempDataMock,
        [Greedy] LineManagerController sut,
        List<Profile> profiles,
        CancellationToken cancellationToken)
    {
        sut.AddContextWithClaim(ClaimTypes.ApprenticeId, Guid.NewGuid().ToString());
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);
        sut.TempData = tempDataMock.Object;
        LineManagerSubmitModel submitModel = new() { HasEmployersApproval = true };
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());
        apiClient.Setup(p => p.GetProfilesByUserType("apprentice", cancellationToken)).ReturnsAsync(new GetProfilesResult { Profiles = profiles });
        apiClient.Setup(c => c.TryCreateMyApprenticeship(It.IsAny<TryCreateMyApprenticeshipRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Response<MyApprenticeship?>(string.Empty, new(HttpStatusCode.NotFound), () => null));

        var result = await sut.Post(submitModel, cancellationToken);

        var redirectResult = result.As<RedirectResult>();
        redirectResult.Should().NotBeNull();
        redirectResult.Url.Contains("accessdenied");
        apiClient.Verify(c => c.TryCreateMyApprenticeship(It.IsAny<TryCreateMyApprenticeshipRequest>(), cancellationToken));
        sessionServiceMock.Verify(s => s.Set(It.IsAny<OnboardingSessionModel>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Post_HasSeenTermsAndHasLineManagerApprovalAndModelIsInitialised_DoesNotInitializesSessionModel(
        [Frozen] Mock<IValidator<LineManagerSubmitModel>> validatorMock,
        [Frozen] Mock<IOuterApiClient> apiClient,
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] LineManagerController sut,
        Mock<ITempDataDictionary> tempDataMock,
        CancellationToken cancellationToken)
    {
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);
        sut.TempData = tempDataMock.Object;
        LineManagerSubmitModel submitModel = new() { HasEmployersApproval = true };
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());
        sessionServiceMock.Setup(s => s.Contains<OnboardingSessionModel>()).Returns(true);

        await sut.Post(submitModel, cancellationToken);

        apiClient.Verify(p => p.GetProfilesByUserType("apprentice", cancellationToken), Times.Never);
        sessionServiceMock.Verify(s => s.Set(It.IsAny<OnboardingSessionModel>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Post_HasSeenTermsAndHasLineManagerApproval_RedirectsToEmployerSearch(
        [Frozen] Mock<IValidator<LineManagerSubmitModel>> validatorMock,
        [Greedy] LineManagerController sut,
        Mock<ITempDataDictionary> tempDataMock,
        CancellationToken cancellationToken)
    {
        sut.AddContextWithClaim(ClaimTypes.ApprenticeId, Guid.NewGuid().ToString());
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);
        sut.TempData = tempDataMock.Object;
        sut.AddUrlHelperMock();
        LineManagerSubmitModel submitModel = new() { HasEmployersApproval = true };
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());

        var result = await sut.Post(submitModel, cancellationToken);

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.Regions);
    }
}