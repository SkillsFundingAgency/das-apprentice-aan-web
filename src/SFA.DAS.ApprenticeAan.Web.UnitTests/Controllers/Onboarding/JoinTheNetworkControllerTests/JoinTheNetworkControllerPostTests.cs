using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.JoinTheNetworkControllerTests;

[TestFixture]
public class JoinTheNetworkControllerPostTests
{
    [MoqAutoData]
    public void Post_ModelStateIsInvalid_ReloadsViewWithValidationErrors(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] JoinTheNetworkController sut,
        [Frozen] JoinTheNetworkSubmitModel submitmodel)
    {
        OnboardingSessionModel sessionModel = new();
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        sut.ModelState.AddModelError("key", "message");

        var result = sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeFalse();

        sessionModel.HasEmployersApproval.Should().BeNull();

        sessionServiceMock.Verify(s => s.Set(sessionModel));

        result.As<ViewResult>().Should().NotBeNull();
        result.As<ViewResult>().ViewName.Should().Be(JoinTheNetworkController.ViewPath);
        result.As<ViewResult>().Model.As<JoinTheNetworkViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Post_ModelStateIsValid_UpdatesSessionModel(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<JoinTheNetworkSubmitModel>> validatorMock,
        [Frozen] JoinTheNetworkSubmitModel submitmodel,
        [Greedy] JoinTheNetworkController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        sessionServiceMock.Object.Set(sessionModel);

        sut.Post(submitmodel);

        sessionServiceMock.Verify(s => s.Set(sessionModel));

        sessionModel.ApprenticeDetails.ReasonForJoiningTheNetwork.Should().Be(submitmodel.ReasonForJoiningTheNetwork);

        sut.ModelState.IsValid.Should().BeTrue();
    }

    [MoqAutoData]
    public void Post_ModelStateIsValid_RedirectsToJoinTheNetworkView(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Frozen] Mock<IValidator<JoinTheNetworkSubmitModel>> validatorMock,
        [Frozen] JoinTheNetworkSubmitModel submitmodel,
        [Greedy] JoinTheNetworkController sut)
    {
        OnboardingSessionModel sessionModel = new();
        ValidationResult validationResult = new();

        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CurrentJobTitle);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        validatorMock.Setup(v => v.Validate(submitmodel)).Returns(validationResult);

        var result = sut.Post(submitmodel);

        sut.ModelState.IsValid.Should().BeTrue();

        result.As<ViewResult>().Should().NotBeNull();
        result.As<ViewResult>().ViewName.Should().Be(JoinTheNetworkController.ViewPath);
        result.As<ViewResult>().Model.As<JoinTheNetworkViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }
}