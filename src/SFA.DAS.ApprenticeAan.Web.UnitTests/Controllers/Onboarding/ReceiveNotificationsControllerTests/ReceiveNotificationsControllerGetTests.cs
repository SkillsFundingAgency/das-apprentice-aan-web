using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticeAan.Web.UnitTests.TestHelpers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.ReceiveNotificationsControllerTests;

[TestFixture]
public class ReceiveNotificationsControllerGetTests
{
    [Test, MoqAutoData]
    public void Get_WhenCalled_ReturnsViewResultWithViewModel(
        [Frozen] Mock<IValidator<ReceiveNotificationsSubmitModel>> validator,
        [Frozen] Mock<ISessionService> mockSessionService,
        string checkYourAnswersUrl,
        string reasonToJoinUrl,
        OnboardingSessionModel sessionModel,
        [Greedy] ReceiveNotificationsController controller)
    {
        mockSessionService.Setup(s => s.Get<OnboardingSessionModel>()).Returns(sessionModel);

        controller.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.CheckYourAnswers, checkYourAnswersUrl);
        controller.AddUrlHelperMock().AddUrlForRoute(RouteNames.Onboarding.ReasonToJoin, reasonToJoinUrl);

        var result = controller.Get() as ViewResult;

        result.Should().NotBeNull();
        var viewModel = result.Model as ReceiveNotificationsViewModel;
        viewModel.Should().NotBeNull();
        viewModel.ReceiveNotifications.Should().Be(sessionModel.ReceiveNotifications);
    }
}
