using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Web.Infrastructure;
using SFA.DAS.ApprenticePortal.SharedUi.Menu;

namespace SFA.DAS.ApprenticeAan.Web.Controllers.Onboarding;

[Authorize]
[Route("onboarding/receive-notifications", Name = RouteNames.Onboarding.ReceiveNotifications)]
[HideNavigationBar(true, true)]
public class ReceiveNotificationsController : Controller
{
    public const string ViewPath = "~/Views/Onboarding/ReceiveNotifications.cshtml";

    [HttpGet]
    public IActionResult Get([FromRoute] string employerAccountId)
    {
        return View(ViewPath);
    }
}
