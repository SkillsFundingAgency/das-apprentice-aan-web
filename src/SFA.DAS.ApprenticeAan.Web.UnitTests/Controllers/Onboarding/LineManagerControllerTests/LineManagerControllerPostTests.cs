using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
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
    public void Post_ModelStateIsInvalid_RedirectsToLineManager(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] LineManagerController sut,
        [Frozen] LineManagerSubmitModel submitmodel)
    {
        OnboardingSessionModel sessionModel = new();
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.TermsAndConditions);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        var result = sut.Post(submitmodel);

        sut.ModelState.AddModelError("key", "message");

        result.As<ViewResult>().Should().NotBeNull();
        result.As<ViewResult>().ViewName.Should().Be(LineManagerController.ViewPath);
        result.As<ViewResult>().Model.As<LineManagerViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }

    [MoqAutoData]
    public void Post_IsValid_RedirectsToLineManager(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] LineManagerController sut,
        [Frozen] LineManagerSubmitModel submitmodel)
    {
        OnboardingSessionModel sessionModel = new();
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.TermsAndConditions);

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);
        var result = sut.Post(submitmodel);
        sessionModel.HasEmployersApproval = (bool)submitmodel.HasEmployersApproval!;

        sessionServiceMock.Verify(s => s.Set(sessionModel), Times.Never);

        sessionModel.HasEmployersApproval.Should().Be((bool)submitmodel.HasEmployersApproval);

        result.As<ViewResult>().Should().NotBeNull();
        result.As<ViewResult>().ViewName.Should().Be(LineManagerController.ViewPath);
        result.As<ViewResult>().Model.As<LineManagerViewModel>().BackLink.Should().Be(TestConstants.DefaultUrl);
    }
}