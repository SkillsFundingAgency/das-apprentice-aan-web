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

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.EmployerSearchControllerTests;

public class EmployerSearchControllerGetTests
{
    [Test]
    [MoqAutoData]
    public void Get_ReturnsView(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] EmployerSearchController sut)
    {
        sut.AddUrlHelperMock();
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(new OnboardingSessionModel());

        var result = sut.Get();

        result.As<ViewResult>().ViewName.Should().Be(EmployerSearchController.ViewPath);
    }

    [Test]
    [MoqAutoData]
    public void Get_SetsManualEntryLinkToEmployerDetails(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] EmployerSearchController sut)
    {
        const string EmployerDetailsUrl = "details.url";
        sut.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.EmployerDetails, EmployerDetailsUrl);
        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(new OnboardingSessionModel());

        var result = sut.Get();

        result.As<ViewResult>().Model.As<EmployerSearchViewModel>().ManualEntryLink.Should().Be(EmployerDetailsUrl);
    }

    [Test]
    [MoqAutoData]
    public void Get_HasSeenPreview_SetsBackLinkToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] EmployerSearchController sut)
    {
        const string CheckYourAnswersUrl = "checkanswers.url";
        const string LineManagerUrl = "linemanager.url";
        sut
            .AddUrlHelperMock()
            .AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers, CheckYourAnswersUrl)
            .AddUrlForRoute(RouteNames.Onboarding.LineManager, LineManagerUrl);
        sessionServiceMock
            .Setup(s => s.Get<OnboardingSessionModel>())
            .Returns(new OnboardingSessionModel() { HasSeenPreview = true });

        var result = sut.Get();

        result.As<ViewResult>().Model.As<EmployerSearchViewModel>().BackLink.Should().Be(CheckYourAnswersUrl);
    }

    [Test]
    [MoqAutoData]
    public void Get_HasNotSeenPreview_SetsBackLinkToCheckYourAnswers(
        [Frozen] Mock<ISessionService> sessionServiceMock,
        [Greedy] EmployerSearchController sut)
    {
        const string CheckYourAnswersUrl = "checkanswers.url";
        const string ConfirmDetailsUrl = "confirmDetails.url";
        sut
            .AddUrlHelperMock()
            .AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers, CheckYourAnswersUrl)
            .AddUrlForRoute(RouteNames.Onboarding.ConfirmDetails, ConfirmDetailsUrl);
        sessionServiceMock
            .Setup(s => s.Get<OnboardingSessionModel>())
            .Returns(new OnboardingSessionModel() { HasSeenPreview = false });

        var result = sut.Get();

        result.As<ViewResult>().Model.As<EmployerSearchViewModel>().BackLink.Should().Be(ConfirmDetailsUrl);
    }
}
