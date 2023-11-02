using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/before-you-start", Name = RouteNames.Onboarding.BeforeYouStart)]
[HideNavigationBar(true, true)]
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