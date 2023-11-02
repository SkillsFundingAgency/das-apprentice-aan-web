using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticeAan.Web.Models.Onboarding;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/terms-and-conditions", Name = RouteNames.Onboarding.TermsAndConditions)]
[HideNavigationBar(true, true)]
public class TermsAndConditionsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/TermsAndConditions.cshtml";

    [HttpGet]
    public IActionResult Get()
    {
        var model = new TermsAndConditionsViewModel()
        {
            BackLink = Url.RouteUrl(RouteNames.Onboarding.BeforeYouStart)!
        };
        return View(ViewPath, model);
    }

    [HttpPost]
    public IActionResult Post()
    {
        if (!TempData.ContainsKey(TempDataKeys.HasSeenTermsAndConditions)) TempData.Add(TempDataKeys.HasSeenTermsAndConditions, true);
        return RedirectToRoute(RouteNames.Onboarding.LineManager);
    }
}