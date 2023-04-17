using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Domain.OuterApi.Responses;
using SFA.DAS.ApprenticeAan.Web.Configuration;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.LineManagerControllerTests;

[TestFixture]
public class LineManagerControllerPostTests
{
    [MoqAutoData]
    public async Task Post_HasNotSeenTermsAndConditions_RedirectToTheStartOfTheJourney(
        [Greedy] LineManagerController sut,
        Mock<ITempDataDictionary> tempDataMock,
        LineManagerSubmitModel submitModel)
    {
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(false);
        sut.TempData = tempDataMock.Object;

        var result = await sut.Post(submitModel);

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.BeforeYouStart);
    }

    [MoqAutoData]
    public async Task Post_ModelStateIsInvalid_ReloadsViewWithValidationErrors(
        [Greedy] LineManagerController sut,
        Mock<ITempDataDictionary> tempDataMock,
        LineManagerSubmitModel submitModel,
        string termsAndConditionsUrl)
    {
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);
        sut.TempData = tempDataMock.Object;
        sut.ModelState.AddModelError("key", "message");
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.TermsAndConditions, termsAndConditionsUrl);

        var result = await sut.Post(submitModel);

        result.As<ViewResult>().ViewName.Should().Be(LineManagerController.ViewPath);
        result.As<ViewResult>().Model.As<LineManagerViewModel>().BackLink.Should().Be(termsAndConditionsUrl);
    }

    [MoqAutoData]
    public async Task Post_DoesNotHaveLineManagersApproval_RedirectsToTheShutterPage(
        [Frozen] ApplicationConfiguration applicationConfiguration,
        [Frozen] Mock<IValidator<LineManagerSubmitModel>> validatorMock,
        [Greedy] LineManagerController sut,
        Mock<ITempDataDictionary> tempDataMock,
        LineManagerSubmitModel submitModel)
    {
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);
        sut.TempData = tempDataMock.Object;
        submitModel.HasEmployersApproval = false;
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());

        var result = await sut.Post(submitModel);

        result.As<ViewResult>().ViewName.Should().Be(LineManagerController.ShutterPageViewPath);
        result.As<ViewResult>().Model.As<ShutterPageViewModel>().ApprenticeHomeUrl.Should().Be(applicationConfiguration.ApplicationUrls.ApprenticeHomeUrl.ToString());
    }

    [MoqAutoData]
    public async Task Post_HasSeenTermsAndHasLineManagerApproval_InitializesSessionModel(
        [Frozen] Mock<IValidator<LineManagerSubmitModel>> validatorMock,
        [Frozen] Mock<IProfileService> profileServiceMock,
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] LineManagerController sut,
        Mock<ITempDataDictionary> tempDataMock,
        LineManagerSubmitModel submitModel,
        List<Profile> profiles)
    {
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);
        sut.TempData = tempDataMock.Object;
        submitModel.HasEmployersApproval = true;
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());
        profileServiceMock.Setup(p => p.GetProfilesByUserType("apprentice")).ReturnsAsync(profiles);

        await sut.Post(submitModel);

        sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m
            => m.HasAcceptedTerms && m.ProfileData.Count == profiles.Count)));
    }

    [MoqAutoData]
    public async Task Post_HasSeenTermsAndHasLineManagerApproval_RedirectsToEmployerSearch(
        [Frozen] Mock<IValidator<LineManagerSubmitModel>> validatorMock,
        [Greedy] LineManagerController sut,
        Mock<ITempDataDictionary> tempDataMock,
        LineManagerSubmitModel submitModel)
    {
        tempDataMock.Setup(t => t.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)).Returns(true);
        sut.TempData = tempDataMock.Object;
        submitModel.HasEmployersApproval = true;
        validatorMock.Setup(v => v.Validate(submitModel)).Returns(new ValidationResult());

        var result = await sut.Post(submitModel);

        result.As<RedirectToRouteResult>().RouteName.Should().Be(RouteNames.Onboarding.EmployerSearch);
    }

}