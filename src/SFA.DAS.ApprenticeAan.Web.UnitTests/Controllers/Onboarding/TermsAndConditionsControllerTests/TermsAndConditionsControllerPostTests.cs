using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Web.UnitTests.Controllers.Onboarding.TermsAndConditionsControllerTests
{
    [TestFixture]
    public class TermsAndConditionsControllerPostTests
    {
        [MoqAutoData]
        public void Post_SetsEmptyOnBoardingSessionModel(
       [Frozen] Mock<ISessionService> sessionServiceMock,
       [Greedy] TermsAndConditionsController sut)
        {
            sut.Post();

            sessionServiceMock.Verify(s => s.Set(It.Is<OnboardingSessionModel>(m => m.HasAcceptedTermsAndConditions)));
        }

        [Test]
        public void GetSessionModelWithEscapeRoute_SessionModelIsMissing_ReturnsEscapeRoute()
        {
            var sessionServiceMock = new Mock<ISessionService>();
            sessionServiceMock.Setup(s => s.Get<OnboardingSessionModel>()).Returns((OnboardingSessionModel)null!);
            var ctrl = new TermsAndConditionsController(sessionServiceMock.Object, Mock.Of<ILogger<TermsAndConditionsController>>());

            var result = ctrl.Post();

            result.As<RedirectToActionResult>().ActionName.Should().Be("Index");
            result.As<RedirectToActionResult>().ControllerName.Should().Be("Home");
        }
    }
}
