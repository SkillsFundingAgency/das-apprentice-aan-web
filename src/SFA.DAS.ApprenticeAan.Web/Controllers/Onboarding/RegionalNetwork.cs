using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Domain.Interfaces;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/regionalNetwork", Name = RouteNames.Onboarding.RegionalNetwork)]
[HideNavigationBar(true, true)]
public class RegionalNetworkController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/RegionalNetwork.cshtml";
    private readonly ISessionService _sessionService;

    public RegionalNetworkController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return View(ViewPath);
    }
}
