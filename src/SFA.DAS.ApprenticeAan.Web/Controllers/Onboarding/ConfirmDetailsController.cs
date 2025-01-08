using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/confirmDetails", Name = RouteNames.Onboarding.ConfirmDetails)]
[HideNavigationBar(true, true)]
public class ConfirmDetailsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/ConfirmDetails.cshtml";

    private readonly ISessionService _sessionService;

    public ConfirmDetailsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(ViewPath);
    }
}

