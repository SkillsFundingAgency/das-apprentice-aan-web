using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Models;
using SFA.DAS.ApprenticeAan.Web.Services;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding
{
    public class BeforeYouStartController : OnboardingControllerBase
    {
        public BeforeYouStartController(ISessionService sessionService) : base(sessionService) { }

        [HttpGet]
        public IActionResult Get()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Post()
        {
            SessionService.Set(new OnboardingSessionModel());
            return Ok();
        }
    }
}
