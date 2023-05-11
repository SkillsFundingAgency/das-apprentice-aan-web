using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/before-you-start", Name = RouteNames.Onboarding.BeforeYouStart)]
public class BeforeYouStartController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/BeforeYouStart.cshtml";

    [HttpGet]
    public IActionResult Get()
    {
        return View(ViewPath);
    }

    [HttpPost]
    public IActionResult Post()
    {
        return RedirectToRoute(RouteNames.Onboarding.TermsAndConditions);
    }
}