using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding;

[TestFixture]
public class OnboardingControllerBaseTests
{
    [Test]
    public void GetSessionModelWithEscapeRoute_ModelExistsInSession_ReturnsModel()
    {
        var sessionServiceMock = new Mock<ISessionService>();

        sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns(new OnboardingSessionModel());

        var sut = new TestController(sessionServiceMock.Object, Mock.Of<ILogger>());

        var result = sut.TestAction();

        result.As<OkObjectResult>().Should().NotBeNull();
    }

    [Test]
    public void GetSessionModelWithEscapeRoute_HasInValidModel_ReturnsRedirectResult()
    {
        var sut = new TestController(Mock.Of<ISessionService>(), Mock.Of<ILogger>());

        var result = sut.TestAction();

        result.As<RedirectToActionResult>().Should().NotBeNull();
    }

    public class TestController : OnboardingControllerBase
    {
        private readonly ILogger _logger;

        public TestController(ISessionService sessionService, ILogger logger) : base(sessionService)
        {
            _logger = logger;
        }

        public IActionResult TestAction()
        {
            var (sessionModel, escapeRoute) = GetSessionModelWithEscapeRoute(_logger);
            return sessionModel == null ? escapeRoute! : Ok(sessionModel);
        }
    }
}
